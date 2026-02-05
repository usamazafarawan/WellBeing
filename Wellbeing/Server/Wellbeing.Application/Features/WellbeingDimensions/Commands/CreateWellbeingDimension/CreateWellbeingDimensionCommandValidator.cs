using FluentValidation;

namespace Wellbeing.Application.Features.WellbeingDimensions.Commands.CreateWellbeingDimension;

public class CreateWellbeingDimensionCommandValidator : AbstractValidator<CreateWellbeingDimensionCommand>
{
    public CreateWellbeingDimensionCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Wellbeing Dimension name is required.")
            .MaximumLength(200).WithMessage("Wellbeing Dimension name must not exceed 200 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Description must not exceed 1000 characters.");

        RuleFor(x => x.ClientsId)
            .GreaterThan(0).WithMessage("Client ID must be greater than 0.");
    }
}
