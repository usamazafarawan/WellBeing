using MediatR;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Wellbeing.Application.DTOs;
using Wellbeing.Application.Common.Interfaces;

namespace Wellbeing.Application.Features.WellbeingDimensions.Queries.GetAllWellbeingDimensions;

public class GetAllWellbeingDimensionsQueryHandler : IRequestHandler<GetAllWellbeingDimensionsQuery, IEnumerable<WellbeingDimensionDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllWellbeingDimensionsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<WellbeingDimensionDto>> Handle(GetAllWellbeingDimensionsQuery request, CancellationToken cancellationToken)
    {
        var wellbeingDimensions = await _context.WellbeingDimensions
            .Include(wd => wd.Clients)
            .Where(x => !x.IsDeleted)
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
