using MediatR;
using Wellbeing.Application.DTOs;

namespace Wellbeing.Application.Features.WellbeingSubDimensions.Queries.GetWellbeingSubDimensionById;

public class GetWellbeingSubDimensionByIdQuery : IRequest<WellbeingSubDimensionDto>
{
    public int Id { get; set; }
}
