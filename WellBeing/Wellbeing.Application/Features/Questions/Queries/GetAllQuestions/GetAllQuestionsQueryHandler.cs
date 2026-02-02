using MediatR;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Wellbeing.Application.DTOs;
using Wellbeing.Application.Common.Interfaces;

namespace Wellbeing.Application.Features.Questions.Queries.GetAllQuestions;

public class GetAllQuestionsQueryHandler : IRequestHandler<GetAllQuestionsQuery, IEnumerable<QuestionDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllQuestionsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<QuestionDto>> Handle(GetAllQuestionsQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Questions
            .Include(q => q.Survey)
            .Include(q => q.WellbeingDimension)
            .Include(q => q.WellbeingSubDimension)
            .Include(q => q.Clients)
            .Where(x => !x.IsDeleted)
            .AsQueryable();

        if (request.SurveyId.HasValue)
        {
            query = query.Where(q => q.SurveyId == request.SurveyId.Value);
        }

        if (request.ClientsId.HasValue)
        {
            query = query.Where(q => q.ClientsId == request.ClientsId.Value);
        }

        var questions = await query
            .OrderBy(q => q.SurveyId)
            .ThenBy(q => q.DisplayOrder)
            .ThenBy(q => q.CreatedAt)
            .ToListAsync(cancellationToken);

        return questions.Select(q =>
        {
            var dto = _mapper.Map<QuestionDto>(q);
            dto.SurveyTitle = q.Survey?.Title;
            dto.WellbeingDimensionName = q.WellbeingDimension?.Name;
            dto.WellbeingSubDimensionName = q.WellbeingSubDimension?.Name;
            dto.ClientsName = q.Clients?.Name;
            return dto;
        });
    }
}
