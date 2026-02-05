using MediatR;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Wellbeing.Application.DTOs;
using Wellbeing.Application.Common.Interfaces;

namespace Wellbeing.Application.Features.Questions.Queries.GetQuestionsBySubDimensionId;

public class GetQuestionsBySubDimensionIdQueryHandler : IRequestHandler<GetQuestionsBySubDimensionIdQuery, IEnumerable<QuestionDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetQuestionsBySubDimensionIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<QuestionDto>> Handle(GetQuestionsBySubDimensionIdQuery request, CancellationToken cancellationToken)
    {
        var questions = await _context.Questions
            .Include(q => q.WellbeingDimension)
            .Include(q => q.WellbeingSubDimension)
            .Include(q => q.Clients)
            .Where(x => x.WellbeingSubDimensionId == request.SubDimensionId && !x.IsDeleted)
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
