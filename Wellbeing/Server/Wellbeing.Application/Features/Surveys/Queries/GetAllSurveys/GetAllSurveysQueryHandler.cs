using MediatR;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Wellbeing.Application.DTOs;
using Wellbeing.Application.Common.Interfaces;

namespace Wellbeing.Application.Features.Surveys.Queries.GetAllSurveys;

public class GetAllSurveysQueryHandler : IRequestHandler<GetAllSurveysQuery, IEnumerable<SurveyDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILoggerService _logger;

    public GetAllSurveysQueryHandler(IApplicationDbContext context, IMapper mapper, ILoggerService logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<SurveyDto>> Handle(GetAllSurveysQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting all surveys");

        var query = _context.Surveys
            .Include(s => s.Clients)
            .Include(s => s.Questions)
            .AsQueryable();

        if (request.ClientsId.HasValue)
        {
            query = query.Where(s => s.ClientsId == request.ClientsId.Value);
        }

        if (request.IsActive.HasValue)
        {
            query = query.Where(s => s.IsActive == request.IsActive.Value);
        }

        var surveys = await query
            .OrderBy(s => s.CreatedAt)
            .ToListAsync(cancellationToken);

        var surveyDtos = _mapper.Map<List<SurveyDto>>(surveys);
        
        foreach (var dto in surveyDtos)
        {
            var survey = surveys.First(s => s.Id == dto.Id);
            dto.QuestionsCount = survey.Questions.Count(q => !q.IsDeleted);
        }

        _logger.LogInformation("Retrieved {Count} surveys", surveyDtos.Count);
        return surveyDtos;
    }
}
