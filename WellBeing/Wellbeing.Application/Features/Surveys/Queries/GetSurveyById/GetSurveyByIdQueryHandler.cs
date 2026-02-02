using MediatR;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Wellbeing.Application.DTOs;
using Wellbeing.Application.Common.Interfaces;

namespace Wellbeing.Application.Features.Surveys.Queries.GetSurveyById;

public class GetSurveyByIdQueryHandler : IRequestHandler<GetSurveyByIdQuery, SurveyDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILoggerService _logger;

    public GetSurveyByIdQueryHandler(IApplicationDbContext context, IMapper mapper, ILoggerService logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<SurveyDto> Handle(GetSurveyByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting survey with ID: {SurveyId}", request.Id);

        var survey = await _context.Surveys
            .Include(s => s.Clients)
            .Include(s => s.Questions)
            .FirstOrDefaultAsync(s => s.Id == request.Id && !s.IsDeleted, cancellationToken);

        if (survey == null)
        {
            _logger.LogWarning("Survey with ID {SurveyId} not found", request.Id);
            throw new KeyNotFoundException($"Survey with ID {request.Id} was not found.");
        }

        var surveyDto = _mapper.Map<SurveyDto>(survey);
        surveyDto.QuestionsCount = survey.Questions.Count(q => !q.IsDeleted);

        return surveyDto;
    }
}
