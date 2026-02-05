using MediatR;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Wellbeing.Application.DTOs;
using Wellbeing.Application.Common.Interfaces;

namespace Wellbeing.Application.Features.Surveys.Commands.UpdateSurvey;

public class UpdateSurveyCommandHandler : IRequestHandler<UpdateSurveyCommand, SurveyDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILoggerService _logger;

    public UpdateSurveyCommandHandler(IApplicationDbContext context, IMapper mapper, ILoggerService logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<SurveyDto> Handle(UpdateSurveyCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating survey with ID: {SurveyId}", request.Id);

        var survey = await _context.Surveys
            .Include(s => s.Questions)
            .FirstOrDefaultAsync(x => x.Id == request.Id && !x.IsDeleted, cancellationToken);

        if (survey == null)
        {
            _logger.LogWarning("Survey with ID {SurveyId} not found or has been deleted", request.Id);
            throw new KeyNotFoundException($"Survey with ID {request.Id} was not found or has been deleted. Please verify the survey ID and try again.");
        }

        survey.Title = request.Title;
        survey.Description = request.Description;
        survey.IsActive = request.IsActive;
        survey.StartDate = request.StartDate;
        survey.EndDate = request.EndDate;
        survey.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Survey with ID {SurveyId} updated successfully", survey.Id);

        var surveyDto = _mapper.Map<SurveyDto>(survey);
        surveyDto.QuestionsCount = survey.Questions.Count(q => !q.IsDeleted);
        return surveyDto;
    }
}
