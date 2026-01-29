using MediatR;
using Wellbeing.Application.DTOs;

namespace Wellbeing.Application.Features.WellbeingSubDimensions.Queries.GetWellbeingSubDimensionsByClientId;

public class GetWellbeingSubDimensionsByClientIdQuery : IRequest<IEnumerable<WellbeingSubDimensionDto>>
{
    public int ClientId { get; set; }
}
