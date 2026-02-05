using MediatR;
using Wellbeing.Application.DTOs;

namespace Wellbeing.Application.Features.WellbeingSubDimensions.Commands.CreateWellbeingSubDimension;

public class CreateWellbeingSubDimensionCommand : IRequest<WellbeingSubDimensionDto>
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int WellbeingDimensionId { get; set; }
    public int ClientsId { get; set; }
}
