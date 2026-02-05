using MediatR;

namespace Wellbeing.Application.Features.WellbeingSubDimensions.Commands.DeleteWellbeingSubDimension;

public class DeleteWellbeingSubDimensionCommand : IRequest<Unit>
{
    public int Id { get; set; }
}
