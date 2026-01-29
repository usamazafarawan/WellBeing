using FluentValidation;

namespace Wellbeing.Application.Features.Questions.Commands.UpdateQuestion;

public class UpdateQuestionCommandValidator : AbstractValidator<UpdateQuestionCommand>
{
    public UpdateQuestionCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Question ID must be greater than 0.");

        RuleFor(x => x.QuestionText)
            .NotEmpty().WithMessage("Question text is required.")
            .MaximumLength(1000).WithMessage("Question text must not exceed 1000 characters.");

        RuleFor(x => x.QuestionType)
            .MaximumLength(50).WithMessage("Question type must not exceed 50 characters.");

        RuleFor(x => x.WellbeingDimensionId)
            .GreaterThan(0).WithMessage("Wellbeing Dimension ID must be greater than 0.");

        RuleFor(x => x.WellbeingSubDimensionId)
            .GreaterThan(0).WithMessage("Wellbeing Sub-Dimension ID must be greater than 0.");

        RuleFor(x => x.ClientsId)
            .GreaterThan(0).WithMessage("Client ID must be greater than 0.");
    }
}
