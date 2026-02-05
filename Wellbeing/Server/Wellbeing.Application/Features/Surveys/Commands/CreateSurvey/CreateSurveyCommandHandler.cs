using MediatR;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Wellbeing.Application.DTOs;
using Wellbeing.Application.Common.Interfaces;
using Wellbeing.Domain.Entities;

namespace Wellbeing.Application.Features.Surveys.Commands.CreateSurvey;

public class CreateSurveyCommandHandler : IRequestHandler<CreateSurveyCommand, SurveyDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILoggerService _logger;

    public CreateSurveyCommandHandler(IApplicationDbContext context, IMapper mapper, ILoggerService logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<SurveyDto> Handle(CreateSurveyCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating new survey with title: {Title}, clientId: {ClientsId}", request.Title, request.ClientsId);

        // Verify client exists
        var clientExists = await _context.Clients
            .AnyAsync(c => c.Id == request.ClientsId && !c.IsDeleted, cancellationToken);

        if (!clientExists)
        {
            _logger.LogWarning("Client with ID {ClientsId} not found or has been deleted", request.ClientsId);
            throw new KeyNotFoundException($"Client with ID {request.ClientsId} was not found or has been deleted. Please verify the client ID and try again.");
        }

        var survey = new Survey
        {
            Title = request.Title,
            Description = request.Description,
            ClientsId = request.ClientsId,
            IsActive = request.IsActive,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            CreatedAt = DateTime.UtcNow,
            IsDeleted = false
        };

        _context.Surveys.Add(survey);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Survey created successfully with ID: {SurveyId}", survey.Id);

        var surveyDto = _mapper.Map<SurveyDto>(survey);
        surveyDto.QuestionsCount = 0;
        return surveyDto;
    }
}
