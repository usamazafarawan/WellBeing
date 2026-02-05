using MediatR;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Wellbeing.Application.DTOs;
using Wellbeing.Application.Common.Interfaces;

namespace Wellbeing.Application.Features.WellbeingSubDimensions.Queries.GetAllWellbeingSubDimensions;

public class GetAllWellbeingSubDimensionsQueryHandler : IRequestHandler<GetAllWellbeingSubDimensionsQuery, IEnumerable<WellbeingSubDimensionDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllWellbeingSubDimensionsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<WellbeingSubDimensionDto>> Handle(GetAllWellbeingSubDimensionsQuery request, CancellationToken cancellationToken)
    {
        var wellbeingSubDimensions = await _context.WellbeingSubDimensions
            .Include(wsd => wsd.WellbeingDimension)
            .Include(wsd => wsd.Clients)
            .Where(x => !x.IsDeleted)
            .OrderBy(x => x.CreatedAt)
            .ToListAsync(cancellationToken);

        return wellbeingSubDimensions.Select(wsd =>
        {
            var dto = _mapper.Map<WellbeingSubDimensionDto>(wsd);
            dto.WellbeingDimensionName = wsd.WellbeingDimension?.Name;
            dto.ClientsName = wsd.Clients?.Name;
            return dto;
        });
    }
}
