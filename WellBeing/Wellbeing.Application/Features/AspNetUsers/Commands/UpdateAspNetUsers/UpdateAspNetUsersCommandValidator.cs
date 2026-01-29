using FluentValidation;

namespace Wellbeing.Application.Features.AspNetUsers.Commands.UpdateAspNetUsers;

public class UpdateAspNetUsersCommandValidator : AbstractValidator<UpdateAspNetUsersCommand>
{
    public UpdateAspNetUsersCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("AspNetUsers ID is required.");

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .MaximumLength(100).WithMessage("First name must not exceed 100 characters.");

        RuleFor(x => x.LastName)
            .MaximumLength(100).WithMessage("Last name must not exceed 100 characters.");

        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("User name is required.")
            .MaximumLength(256).WithMessage("User name must not exceed 256 characters.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Email is not valid.")
            .MaximumLength(256).WithMessage("Email must not exceed 256 characters.");

        RuleFor(x => x.ClientsId)
            .GreaterThan(0).WithMessage("Clients ID must be greater than 0.");

        RuleFor(x => x.PhoneNumber)
            .MaximumLength(20).WithMessage("Phone number must not exceed 20 characters.")
            .When(x => !string.IsNullOrEmpty(x.PhoneNumber));

        RuleFor(x => x.LeadershipLevel)
            .MaximumLength(100).WithMessage("Leadership level must not exceed 100 characters.")
            .When(x => !string.IsNullOrEmpty(x.LeadershipLevel));

        RuleFor(x => x.Tenant)
            .MaximumLength(100).WithMessage("Tenant must not exceed 100 characters.")
            .When(x => !string.IsNullOrEmpty(x.Tenant));
    }
}
