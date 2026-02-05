using MediatR;
using Wellbeing.Application.DTOs;

namespace Wellbeing.Application.Features.WellbeingDimensions.Queries.GetAllWellbeingDimensions;

public class GetAllWellbeingDimensionsQuery : IRequest<IEnumerable<WellbeingDimensionDto>>
{
}
