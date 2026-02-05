using MediatR;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Wellbeing.Application.DTOs;
using Wellbeing.Application.Common.Interfaces;

namespace Wellbeing.Application.Features.Questions.Queries.GetQuestionsByDimensionId;

public class GetQuestionsByDimensionIdQueryHandler : IRequestHandler<GetQuestionsByDimensionIdQuery, IEnumerable<QuestionDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetQuestionsByDimensionIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<QuestionDto>> Handle(GetQuestionsByDimensionIdQuery request, CancellationToken cancellationToken)
    {
        var questions = await _context.Questions
            .Include(q => q.WellbeingDimension)
            .Include(q => q.WellbeingSubDimension)
            .Include(q => q.Clients)
            .Where(x => x.WellbeingDimensionId == request.DimensionId && !x.IsDeleted)
            .OrderBy(x => x.CreatedAt)
            .ToListAsync(cancellationToken);

        return questions.Select(q =>
        {
            var dto = _mapper.Map<QuestionDto>(q);
            dto.WellbeingDimensionName = q.WellbeingDimension?.Name;
            dto.WellbeingSubDimensionName = q.WellbeingSubDimension?.Name;
            dto.ClientsName = q.Clients?.Name;
            return dto;
        });
    }
}
