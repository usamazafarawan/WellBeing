using MediatR;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Wellbeing.Application.DTOs;
using Wellbeing.Application.Common.Interfaces;

namespace Wellbeing.Application.Features.WellbeingSubDimensions.Queries.GetWellbeingSubDimensionsByDimensionId;

public class GetWellbeingSubDimensionsByDimensionIdQueryHandler : IRequestHandler<GetWellbeingSubDimensionsByDimensionIdQuery, IEnumerable<WellbeingSubDimensionDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetWellbeingSubDimensionsByDimensionIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<WellbeingSubDimensionDto>> Handle(GetWellbeingSubDimensionsByDimensionIdQuery request, CancellationToken cancellationToken)
    {
        var wellbeingSubDimensions = await _context.WellbeingSubDimensions
            .Include(wsd => wsd.WellbeingDimension)
            .Include(wsd => wsd.Clients)
            .Where(x => x.WellbeingDimensionId == request.DimensionId && !x.IsDeleted)
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
