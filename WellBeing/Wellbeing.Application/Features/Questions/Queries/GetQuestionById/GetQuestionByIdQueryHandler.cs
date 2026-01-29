using MediatR;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Wellbeing.Application.DTOs;
using Wellbeing.Application.Common.Interfaces;

namespace Wellbeing.Application.Features.Questions.Queries.GetQuestionById;

public class GetQuestionByIdQueryHandler : IRequestHandler<GetQuestionByIdQuery, QuestionDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetQuestionByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<QuestionDto> Handle(GetQuestionByIdQuery request, CancellationToken cancellationToken)
    {
        var question = await _context.Questions
            .Include(q => q.WellbeingDimension)
            .Include(q => q.WellbeingSubDimension)
            .Include(q => q.Clients)
            .FirstOrDefaultAsync(x => x.Id == request.Id && !x.IsDeleted, cancellationToken);

        if (question == null)
        {
            throw new KeyNotFoundException($"Question with ID {request.Id} not found.");
        }

        var dto = _mapper.Map<QuestionDto>(question);
        dto.WellbeingDimensionName = question.WellbeingDimension?.Name;
        dto.WellbeingSubDimensionName = question.WellbeingSubDimension?.Name;
        dto.ClientsName = question.Clients?.Name;
        return dto;
    }
}
