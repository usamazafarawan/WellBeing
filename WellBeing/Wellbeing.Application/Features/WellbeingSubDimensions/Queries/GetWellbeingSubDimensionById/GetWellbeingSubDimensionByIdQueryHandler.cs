using MediatR;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Wellbeing.Application.DTOs;
using Wellbeing.Application.Common.Interfaces;

namespace Wellbeing.Application.Features.WellbeingSubDimensions.Queries.GetWellbeingSubDimensionById;

public class GetWellbeingSubDimensionByIdQueryHandler : IRequestHandler<GetWellbeingSubDimensionByIdQuery, WellbeingSubDimensionDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetWellbeingSubDimensionByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<WellbeingSubDimensionDto> Handle(GetWellbeingSubDimensionByIdQuery request, CancellationToken cancellationToken)
    {
        var wellbeingSubDimension = await _context.WellbeingSubDimensions
            .Include(wsd => wsd.WellbeingDimension)
            .Include(wsd => wsd.Clients)
            .FirstOrDefaultAsync(x => x.Id == request.Id && !x.IsDeleted, cancellationToken);

        if (wellbeingSubDimension == null)
        {
            throw new KeyNotFoundException($"Wellbeing Sub-Dimension with ID {request.Id} not found.");
        }

        var dto = _mapper.Map<WellbeingSubDimensionDto>(wellbeingSubDimension);
        dto.WellbeingDimensionName = wellbeingSubDimension.WellbeingDimension?.Name;
        dto.ClientsName = wellbeingSubDimension.Clients?.Name;
        return dto;
    }
}
