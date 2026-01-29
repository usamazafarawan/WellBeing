using MediatR;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Wellbeing.Application.DTOs;
using Wellbeing.Application.Common.Interfaces;

namespace Wellbeing.Application.Features.WellbeingDimensions.Queries.GetWellbeingDimensionsByClientId;

public class GetWellbeingDimensionsByClientIdQueryHandler : IRequestHandler<GetWellbeingDimensionsByClientIdQuery, IEnumerable<WellbeingDimensionDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetWellbeingDimensionsByClientIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<WellbeingDimensionDto>> Handle(GetWellbeingDimensionsByClientIdQuery request, CancellationToken cancellationToken)
    {
        var wellbeingDimensions = await _context.WellbeingDimensions
            .Include(wd => wd.Clients)
            .Where(x => x.ClientsId == request.ClientId && !x.IsDeleted)
            .OrderBy(x => x.CreatedAt)
            .ToListAsync(cancellationToken);

        return wellbeingDimensions.Select(wd =>
        {
            var dto = _mapper.Map<WellbeingDimensionDto>(wd);
            dto.ClientsName = wd.Clients?.Name;
            return dto;
        });
    }
}
