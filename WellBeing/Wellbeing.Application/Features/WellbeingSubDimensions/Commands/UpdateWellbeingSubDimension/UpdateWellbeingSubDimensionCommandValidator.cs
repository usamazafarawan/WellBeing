using FluentValidation;

namespace Wellbeing.Application.Features.WellbeingSubDimensions.Commands.UpdateWellbeingSubDimension;

public class UpdateWellbeingSubDimensionCommandValidator : AbstractValidator<UpdateWellbeingSubDimensionCommand>
{
    public UpdateWellbeingSubDimensionCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Wellbeing Sub-Dimension ID must be greater than 0.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Wellbeing Sub-Dimension name is required.")
            .MaximumLength(200).WithMessage("Wellbeing Sub-Dimension name must not exceed 200 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Description must not exceed 1000 characters.");

        RuleFor(x => x.WellbeingDimensionId)
            .GreaterThan(0).WithMessage("Wellbeing Dimension ID must be greater than 0.");

        RuleFor(x => x.ClientsId)
            .GreaterThan(0).WithMessage("Client ID must be greater than 0.");
    }
}
