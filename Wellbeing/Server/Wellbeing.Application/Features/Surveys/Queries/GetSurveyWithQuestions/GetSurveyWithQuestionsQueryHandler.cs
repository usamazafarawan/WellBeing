using MediatR;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Wellbeing.Application.Common.Interfaces;
using Wellbeing.Application.DTOs;

namespace Wellbeing.Application.Features.Surveys.Queries.GetSurveyWithQuestions;

public class GetSurveyWithQuestionsQueryHandler : IRequestHandler<GetSurveyWithQuestionsQuery, SurveyWithQuestionsDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILoggerService _logger;

    public GetSurveyWithQuestionsQueryHandler(IApplicationDbContext context, IMapper mapper, ILoggerService logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<SurveyWithQuestionsDto> Handle(GetSurveyWithQuestionsQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting survey with questions for ID: {SurveyId}", request.Id);

        var survey = await _context.Surveys
            .Include(s => s.Clients)
            .Include(s => s.Questions.Where(q => !q.IsDeleted))
                .ThenInclude(q => q.WellbeingDimension)
            .Include(s => s.Questions.Where(q => !q.IsDeleted))
                .ThenInclude(q => q.WellbeingSubDimension)
            .FirstOrDefaultAsync(s => s.Id == request.Id && !s.IsDeleted, cancellationToken);

        if (survey == null)
        {
            _logger.LogWarning("Survey with ID {SurveyId} not found", request.Id);
            throw new KeyNotFoundException($"Survey with ID {request.Id} was not found.");
        }

        var surveyDto = new SurveyWithQuestionsDto
        {
            Id = survey.Id,
            Title = survey.Title,
            Description = survey.Description,
            ClientsId = survey.ClientsId,
            ClientsName = survey.Clients.Name,
            IsActive = survey.IsActive,
            StartDate = survey.StartDate,
            EndDate = survey.EndDate,
            CreatedAt = survey.CreatedAt,
            ModifiedAt = survey.UpdatedAt,
            Questions = _mapper.Map<List<QuestionDto>>(survey.Questions.OrderBy(q => q.DisplayOrder))
        };

        return surveyDto;
    }
}
