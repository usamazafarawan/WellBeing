using MediatR;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Wellbeing.Application.DTOs;
using Wellbeing.Application.Common.Interfaces;

namespace Wellbeing.Application.Features.WellbeingDimensions.Queries.GetWellbeingDimensionById;

public class GetWellbeingDimensionByIdQueryHandler : IRequestHandler<GetWellbeingDimensionByIdQuery, WellbeingDimensionDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetWellbeingDimensionByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<WellbeingDimensionDto> Handle(GetWellbeingDimensionByIdQuery request, CancellationToken cancellationToken)
    {
        var wellbeingDimension = await _context.WellbeingDimensions
            .Include(wd => wd.Clients)
            .FirstOrDefaultAsync(x => x.Id == request.Id && !x.IsDeleted, cancellationToken);

        if (wellbeingDimension == null)
        {
            throw new KeyNotFoundException($"Wellbeing Dimension with ID {request.Id} was not found or has been deleted.");
        }

        var dto = _mapper.Map<WellbeingDimensionDto>(wellbeingDimension);
        dto.ClientsName = wellbeingDimension.Clients?.Name;
        return dto;
    }
}
