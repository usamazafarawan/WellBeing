using MediatR;
using Wellbeing.Application.DTOs;

namespace Wellbeing.Application.Features.WellbeingDimensions.Queries.GetWellbeingDimensionsByClientId;

public class GetWellbeingDimensionsByClientIdQuery : IRequest<IEnumerable<WellbeingDimensionDto>>
{
    public int ClientId { get; set; }
}
