using MediatR;
using Wellbeing.Application.DTOs;

namespace Wellbeing.Application.Features.WellbeingSubDimensions.Queries.GetAllWellbeingSubDimensions;

public class GetAllWellbeingSubDimensionsQuery : IRequest<IEnumerable<WellbeingSubDimensionDto>>
{
}
