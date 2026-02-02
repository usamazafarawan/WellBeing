using FluentValidation;
using System.Text.Json;

namespace Wellbeing.Application.Features.Questions.Commands.UpdateQuestion;

public class UpdateQuestionCommandValidator : AbstractValidator<UpdateQuestionCommand>
{
    public UpdateQuestionCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Question ID is required and must be greater than 0.");

        RuleFor(x => x.QuestionText)
            .NotEmpty().WithMessage("Question text is required and cannot be empty.")
            .MaximumLength(2000).WithMessage("Question text must not exceed 2000 characters.");

        RuleFor(x => x.QuestionType)
            .MaximumLength(50).WithMessage("Question type must not exceed 50 characters.")
            .When(x => !string.IsNullOrEmpty(x.QuestionType));

        RuleFor(x => x.SurveyId)
            .GreaterThan(0).WithMessage("Survey ID is required and must be greater than 0.");

        RuleFor(x => x.DisplayOrder)
            .GreaterThanOrEqualTo(0).WithMessage("Display order must be 0 or greater.");

        RuleFor(x => x.WellbeingDimensionId)
            .GreaterThan(0).WithMessage("Wellbeing Dimension ID must be greater than 0 when provided.")
            .When(x => x.WellbeingDimensionId.HasValue);

        RuleFor(x => x.WellbeingSubDimensionId)
            .GreaterThan(0).WithMessage("Wellbeing Sub-Dimension ID must be greater than 0 when provided.")
            .When(x => x.WellbeingSubDimensionId.HasValue);

        RuleFor(x => x.QuestionConfig)
            .Must(BeValidJson).WithMessage("QuestionConfig must be valid JSON format.")
            .When(x => !string.IsNullOrWhiteSpace(x.QuestionConfig));

        RuleFor(x => x.QuestionConfig)
            .Must(HaveValidComponentStructure).WithMessage("QuestionConfig must contain a valid 'components' array with at least one component.")
            .When(x => !string.IsNullOrWhiteSpace(x.QuestionConfig));
    }

    private bool BeValidJson(string? json)
    {
        if (string.IsNullOrWhiteSpace(json))
            return true;

        try
        {
            JsonDocument.Parse(json);
            return true;
        }
        catch
        {
            return false;
        }
    }

    private bool HaveValidComponentStructure(string? json)
    {
        if (string.IsNullOrWhiteSpace(json))
            return true;

        try
        {
            var doc = JsonDocument.Parse(json);
            if (doc.RootElement.TryGetProperty("components", out var components) && components.ValueKind == JsonValueKind.Array)
            {
                return components.GetArrayLength() > 0;
            }
            return false;
        }
        catch
        {
            return false;
        }
    }
}
