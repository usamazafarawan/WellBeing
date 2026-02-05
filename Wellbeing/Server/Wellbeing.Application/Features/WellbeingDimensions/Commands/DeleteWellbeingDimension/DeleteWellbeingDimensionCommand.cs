using MediatR;

namespace Wellbeing.Application.Features.WellbeingDimensions.Commands.DeleteWellbeingDimension;

public class DeleteWellbeingDimensionCommand : IRequest<Unit>
{
    public int Id { get; set; }
}
