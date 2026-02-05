using MediatR;
using Wellbeing.Application.DTOs;

namespace Wellbeing.Application.Features.WellbeingDimensions.Commands.CreateWellbeingDimension;

public class CreateWellbeingDimensionCommand : IRequest<WellbeingDimensionDto>
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int ClientsId { get; set; }
}
