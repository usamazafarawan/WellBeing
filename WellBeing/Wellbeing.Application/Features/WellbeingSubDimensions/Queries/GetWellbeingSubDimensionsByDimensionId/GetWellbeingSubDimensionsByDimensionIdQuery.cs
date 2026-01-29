using MediatR;
using Wellbeing.Application.DTOs;

namespace Wellbeing.Application.Features.WellbeingSubDimensions.Queries.GetWellbeingSubDimensionsByDimensionId;

public class GetWellbeingSubDimensionsByDimensionIdQuery : IRequest<IEnumerable<WellbeingSubDimensionDto>>
{
    public int DimensionId { get; set; }
}
