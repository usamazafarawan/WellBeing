using FluentValidation;

namespace Wellbeing.Application.Features.Surveys.Commands.UpdateSurvey;

public class UpdateSurveyCommandValidator : AbstractValidator<UpdateSurveyCommand>
{
    public UpdateSurveyCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Survey ID is required and must be greater than 0.");

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Survey title is required and cannot be empty.")
            .MaximumLength(500).WithMessage("Survey title must not exceed 500 characters.")
            .MinimumLength(3).WithMessage("Survey title must be at least 3 characters long.");

        RuleFor(x => x.Description)
            .MaximumLength(2000).WithMessage("Description must not exceed 2000 characters.")
            .When(x => !string.IsNullOrEmpty(x.Description));

        RuleFor(x => x.StartDate)
            .LessThanOrEqualTo(x => DateTime.UtcNow.AddYears(1))
            .WithMessage("Start date cannot be more than 1 year in the future.")
            .When(x => x.StartDate.HasValue);

        RuleFor(x => x.EndDate)
            .GreaterThan(x => x.StartDate)
            .WithMessage("End date must be after start date.")
            .When(x => x.StartDate.HasValue && x.EndDate.HasValue);

        RuleFor(x => x.EndDate)
            .LessThanOrEqualTo(x => DateTime.UtcNow.AddYears(10))
            .WithMessage("End date cannot be more than 10 years in the future.")
            .When(x => x.EndDate.HasValue);
    }
}
