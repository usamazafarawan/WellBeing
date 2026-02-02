using MediatR;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Wellbeing.Application.DTOs;
using Wellbeing.Application.Common.Interfaces;
using System.Text.Json;

namespace Wellbeing.Application.Features.Questions.Commands.UpdateQuestion;

public class UpdateQuestionCommandHandler : IRequestHandler<UpdateQuestionCommand, QuestionDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILoggerService _logger;

    public UpdateQuestionCommandHandler(IApplicationDbContext context, IMapper mapper, ILoggerService logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<QuestionDto> Handle(UpdateQuestionCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating Question with ID: {Id}", request.Id);

        var question = await _context.Questions
            .FirstOrDefaultAsync(x => x.Id == request.Id && !x.IsDeleted, cancellationToken);

        if (question == null)
        {
            _logger.LogWarning("Question with ID {Id} not found or has been deleted", request.Id);
            throw new KeyNotFoundException($"Question with ID {request.Id} was not found or has been deleted. Please verify the question ID and try again.");
        }

        // Validate Survey exists and belongs to the same client
        var survey = await _context.Surveys
            .FirstOrDefaultAsync(s => s.Id == request.SurveyId && s.ClientsId == question.ClientsId && !s.IsDeleted, cancellationToken);
        
        if (survey == null)
        {
            _logger.LogWarning("Survey with ID {SurveyId} not found or does not belong to client {ClientsId}", request.SurveyId, question.ClientsId);
            throw new KeyNotFoundException($"Survey with ID {request.SurveyId} was not found or does not belong to the question's client. Please verify the survey ID and try again.");
        }

        // Validate QuestionConfig JSON if provided
        if (!string.IsNullOrWhiteSpace(request.QuestionConfig))
        {
            try
            {
                using var doc = JsonDocument.Parse(request.QuestionConfig);
                question.QuestionConfig = request.QuestionConfig;
            }
            catch (JsonException ex)
            {
                _logger.LogWarning("Invalid JSON in QuestionConfig: {Error}", ex.Message);
                throw new ArgumentException($"QuestionConfig must be valid JSON format. Error: {ex.Message}", ex);
            }
        }
        else if (request.QuestionConfig == null)
        {
            // Keep existing config if not provided
        }
        else
        {
            // Empty string means clear the config
            question.QuestionConfig = null;
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

        question.QuestionText = request.QuestionText;
        question.QuestionType = request.QuestionType;
        question.SurveyId = request.SurveyId;
        question.IsRequired = request.IsRequired;
        question.DisplayOrder = request.DisplayOrder;
        question.WellbeingDimensionId = request.WellbeingDimensionId;
        question.WellbeingSubDimensionId = request.WellbeingSubDimensionId;
        question.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Question updated successfully with ID: {Id}", question.Id);

        var dto = _mapper.Map<QuestionDto>(question);
        return dto;
    }
}
