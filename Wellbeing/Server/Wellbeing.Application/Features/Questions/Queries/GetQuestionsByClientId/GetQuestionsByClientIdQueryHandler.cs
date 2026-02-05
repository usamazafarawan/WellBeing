using MediatR;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Wellbeing.Application.DTOs;
using Wellbeing.Application.Common.Interfaces;

namespace Wellbeing.Application.Features.Questions.Queries.GetQuestionsByClientId;

public class GetQuestionsByClientIdQueryHandler : IRequestHandler<GetQuestionsByClientIdQuery, IEnumerable<QuestionDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetQuestionsByClientIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<QuestionDto>> Handle(GetQuestionsByClientIdQuery request, CancellationToken cancellationToken)
    {
        var questions = await _context.Questions
            .Include(q => q.WellbeingDimension)
            .Include(q => q.WellbeingSubDimension)
            .Include(q => q.Clients)
            .Where(x => x.ClientsId == request.ClientId && !x.IsDeleted)
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
