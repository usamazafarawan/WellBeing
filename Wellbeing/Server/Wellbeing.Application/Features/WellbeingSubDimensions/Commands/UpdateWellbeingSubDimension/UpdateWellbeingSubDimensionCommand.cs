using MediatR;
using Wellbeing.Application.DTOs;

namespace Wellbeing.Application.Features.WellbeingSubDimensions.Commands.UpdateWellbeingSubDimension;

public class UpdateWellbeingSubDimensionCommand : IRequest<WellbeingSubDimensionDto>
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int WellbeingDimensionId { get; set; }
    public int ClientsId { get; set; }
}
