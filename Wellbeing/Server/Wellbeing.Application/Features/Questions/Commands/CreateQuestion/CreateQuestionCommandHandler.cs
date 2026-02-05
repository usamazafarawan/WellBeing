using MediatR;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Wellbeing.Application.DTOs;
using Wellbeing.Application.Common.Interfaces;
using QuestionEntity = Wellbeing.Domain.Entities.Question;
using System.Text.Json;

namespace Wellbeing.Application.Features.Questions.Commands.CreateQuestion;

public class CreateQuestionCommandHandler : IRequestHandler<CreateQuestionCommand, QuestionDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILoggerService _logger;

    public CreateQuestionCommandHandler(IApplicationDbContext context, IMapper mapper, ILoggerService logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<QuestionDto> Handle(CreateQuestionCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating new Question with text: {QuestionText}, SurveyId: {SurveyId}, ClientId: {ClientsId}", 
            request.QuestionText, request.SurveyId, request.ClientsId);

        // Validate Survey exists and belongs to the client
        var survey = await _context.Surveys
            .FirstOrDefaultAsync(s => s.Id == request.SurveyId && s.ClientsId == request.ClientsId && !s.IsDeleted, cancellationToken);
        
        if (survey == null)
        {
            _logger.LogWarning("Survey with ID {SurveyId} not found or does not belong to client {ClientsId}", request.SurveyId, request.ClientsId);
            throw new KeyNotFoundException($"Survey with ID {request.SurveyId} was not found or does not belong to the specified client. Please verify the survey ID and client ID, then try again.");
        }

        // Validate Client exists
        var client = await _context.Clients
            .FirstOrDefaultAsync(c => c.Id == request.ClientsId && !c.IsDeleted, cancellationToken);
        
        if (client == null)
        {
            _logger.LogWarning("Client with ID {ClientsId} not found or has been deleted", request.ClientsId);
            throw new KeyNotFoundException($"Client with ID {request.ClientsId} was not found or has been deleted. Please verify the client ID and try again.");
        }

        // Validate QuestionConfig JSON if provided
        string? validQuestionConfig = null;
        if (!string.IsNullOrWhiteSpace(request.QuestionConfig))
        {
            try
            {
                using var doc = JsonDocument.Parse(request.QuestionConfig);
                validQuestionConfig = request.QuestionConfig;
            }
            catch (JsonException ex)
            {
                _logger.LogWarning("Invalid JSON in QuestionConfig: {Error}", ex.Message);
                throw new ArgumentException($"QuestionConfig must be valid JSON format. Error: {ex.Message}", ex);
            }
        }

        // Validate optional WellbeingDimension if provided
        if (request.WellbeingDimensionId.HasValue)
        {
            var dimension = await _context.WellbeingDimensions
                .FirstOrDefaultAsync(d => d.Id == request.WellbeingDimensionId.Value && !d.IsDeleted, cancellationToken);
            
            if (dimension == null)
            {
                _logger.LogWarning("WellbeingDimension with ID {WellbeingDimensionId} not found or has been deleted", request.WellbeingDimensionId.Value);
                throw new KeyNotFoundException($"Wellbeing Dimension with ID {request.WellbeingDimensionId.Value} was not found or has been deleted. Please verify the dimension ID and try again.");
            }
        }

        // Validate optional WellbeingSubDimension if provided
        if (request.WellbeingSubDimensionId.HasValue)
        {
            var subDimension = await _context.WellbeingSubDimensions
                .FirstOrDefaultAsync(sd => sd.Id == request.WellbeingSubDimensionId.Value && !sd.IsDeleted, cancellationToken);
            
            if (subDimension == null)
            {
                _logger.LogWarning("WellbeingSubDimension with ID {WellbeingSubDimensionId} not found or has been deleted", request.WellbeingSubDimensionId.Value);
                throw new KeyNotFoundException($"Wellbeing Sub-Dimension with ID {request.WellbeingSubDimensionId.Value} was not found or has been deleted. Please verify the sub-dimension ID and try again.");
            }
        }

        var question = new QuestionEntity
        {
            QuestionText = request.QuestionText,
            QuestionType = request.QuestionType,
            SurveyId = request.SurveyId,
            ClientsId = request.ClientsId,
            QuestionConfig = validQuestionConfig,
            IsRequired = request.IsRequired,
            DisplayOrder = request.DisplayOrder,
            WellbeingDimensionId = request.WellbeingDimensionId,
            WellbeingSubDimensionId = request.WellbeingSubDimensionId,
            CreatedAt = DateTime.UtcNow,
            IsDeleted = false
        };

        _context.Questions.Add(question);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Question created successfully with ID: {Id}", question.Id);

        var dto = _mapper.Map<QuestionDto>(question);
        return dto;
    }
}
