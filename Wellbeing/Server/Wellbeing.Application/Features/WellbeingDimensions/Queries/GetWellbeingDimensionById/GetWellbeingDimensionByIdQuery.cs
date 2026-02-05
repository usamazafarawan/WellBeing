using MediatR;
using Wellbeing.Application.DTOs;

namespace Wellbeing.Application.Features.WellbeingDimensions.Queries.GetWellbeingDimensionById;

public class GetWellbeingDimensionByIdQuery : IRequest<WellbeingDimensionDto>
{
    public int Id { get; set; }
}
