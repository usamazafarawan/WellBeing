using FluentValidation;

namespace Wellbeing.Application.Features.Clients.Commands.CreateClients;

public class CreateClientsCommandValidator : AbstractValidator<CreateClientsCommand>
{
    public CreateClientsCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Clients name is required.")
            .MaximumLength(200).WithMessage("Clients name must not exceed 200 characters.");

        RuleFor(x => x.Domain)
            .NotEmpty().WithMessage("Domain is required.")
            .MaximumLength(255).WithMessage("Domain must not exceed 255 characters.")
            .Matches(@"^([a-zA-Z0-9]([a-zA-Z0-9\-]{0,61}[a-zA-Z0-9])?\.)+[a-zA-Z]{2,}$")
            .WithMessage("Domain must be a valid domain name.");

        RuleFor(x => x.InstructionsText)
            .NotEmpty().WithMessage("Instructions text is required.")
            .MaximumLength(5000).WithMessage("Instructions text must not exceed 5000 characters.");
    }
}
