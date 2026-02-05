using MediatR;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Wellbeing.Application.DTOs;
using Wellbeing.Application.Common.Interfaces;
using Wellbeing.Domain.Entities;
using System.Text.Json;

namespace Wellbeing.Application.Features.QuestionResponses.Commands.SubmitQuestionResponse;

public class SubmitQuestionResponseCommandHandler : IRequestHandler<SubmitQuestionResponseCommand, QuestionResponseDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILoggerService _logger;

    public SubmitQuestionResponseCommandHandler(IApplicationDbContext context, IMapper mapper, ILoggerService logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<QuestionResponseDto> Handle(SubmitQuestionResponseCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Submitting response for QuestionId: {QuestionId}, ComponentType: {ComponentType}", 
            request.QuestionId, request.ComponentType);

        // Validate Question exists
        var question = await _context.Questions
            .FirstOrDefaultAsync(q => q.Id == request.QuestionId && !q.IsDeleted, cancellationToken);

        if (question == null)
        {
            _logger.LogWarning("Question with ID {QuestionId} not found or has been deleted", request.QuestionId);
            throw new KeyNotFoundException($"Question with ID {request.QuestionId} was not found or has been deleted. Please verify the question ID and try again.");
        }

        // Validate that question belongs to the specified client
        if (question.ClientsId != request.ClientsId)
        {
            _logger.LogWarning("Question {QuestionId} does not belong to client {ClientsId}", request.QuestionId, request.ClientsId);
            throw new InvalidOperationException($"Question with ID {request.QuestionId} does not belong to the specified client. Please verify the client ID and try again.");
        }

        // Validate User exists
        var user = await _context.AspNetUsers
            .FirstOrDefaultAsync(u => u.Id == request.AspNetUsersId && !u.IsDeleted, cancellationToken);

        if (user == null)
        {
            _logger.LogWarning("User with ID {AspNetUsersId} not found or has been deleted", request.AspNetUsersId);
            throw new KeyNotFoundException($"User with ID {request.AspNetUsersId} was not found or has been deleted. Please verify the user ID and try again.");
        }

        // Validate that user belongs to the specified client
        if (user.ClientsId != request.ClientsId)
        {
            _logger.LogWarning("User {AspNetUsersId} does not belong to client {ClientsId}", request.AspNetUsersId, request.ClientsId);
            throw new InvalidOperationException($"User with ID {request.AspNetUsersId} does not belong to the specified client. Please verify the client ID and try again.");
        }

        // Validate Client exists
        var client = await _context.Clients
            .FirstOrDefaultAsync(c => c.Id == request.ClientsId && !c.IsDeleted, cancellationToken);

        if (client == null)
        {
            _logger.LogWarning("Client with ID {ClientsId} not found or has been deleted", request.ClientsId);
            throw new KeyNotFoundException($"Client with ID {request.ClientsId} was not found or has been deleted. Please verify the client ID and try again.");
        }

        // Validate ResponseValue is valid JSON
        JsonElement responseJson;
        try
        {
            responseJson = JsonDocument.Parse(request.ResponseValue).RootElement;
        }
        catch (JsonException ex)
        {
            _logger.LogWarning("Invalid JSON in ResponseValue: {Error}", ex.Message);
            throw new ArgumentException($"ResponseValue must be valid JSON format. Error: {ex.Message}", ex);
        }

        // Check if response already exists for this user/question/component
        var existingResponse = await _context.QuestionResponses
            .FirstOrDefaultAsync(r => 
                r.QuestionId == request.QuestionId &&
                r.AspNetUsersId == request.AspNetUsersId &&
                r.ComponentIndex == request.ComponentIndex &&
                !r.IsDeleted, 
                cancellationToken);

        QuestionResponse response;

        if (existingResponse != null)
        {
            // Update existing response
            existingResponse.ComponentType = request.ComponentType;
            existingResponse.ResponseValue = request.ResponseValue;
            existingResponse.UpdatedAt = DateTime.UtcNow;
            response = existingResponse;
            _logger.LogInformation("Updated existing response with ID: {ResponseId}", response.Id);
        }
        else
        {
            // Create new response
            response = new QuestionResponse
            {
                QuestionId = request.QuestionId,
                AspNetUsersId = request.AspNetUsersId,
                ClientsId = request.ClientsId,
                ComponentType = request.ComponentType,
                ComponentIndex = request.ComponentIndex,
                ResponseValue = request.ResponseValue,
                CreatedAt = DateTime.UtcNow,
                IsDeleted = false
            };
            _context.QuestionResponses.Add(response);
            _logger.LogInformation("Created new response");
        }

        await _context.SaveChangesAsync(cancellationToken);

        var responseDto = _mapper.Map<QuestionResponseDto>(response);
        responseDto.QuestionText = question.QuestionText;
        responseDto.UserName = user.UserName;

        return responseDto;
    }
}
