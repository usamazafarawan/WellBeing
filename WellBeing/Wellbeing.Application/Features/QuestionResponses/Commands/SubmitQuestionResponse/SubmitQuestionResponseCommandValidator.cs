using FluentValidation;
using System.Text.Json;

namespace Wellbeing.Application.Features.QuestionResponses.Commands.SubmitQuestionResponse;

public class SubmitQuestionResponseCommandValidator : AbstractValidator<SubmitQuestionResponseCommand>
{
    public SubmitQuestionResponseCommandValidator()
    {
        RuleFor(x => x.QuestionId)
            .GreaterThan(0).WithMessage("Question ID is required and must be greater than 0.");

        RuleFor(x => x.AspNetUsersId)
            .NotEmpty().WithMessage("User ID is required and cannot be empty.");

        RuleFor(x => x.ClientsId)
            .GreaterThan(0).WithMessage("Client ID is required and must be greater than 0.");

        RuleFor(x => x.ComponentType)
            .NotEmpty().WithMessage("Component type is required and cannot be empty.")
            .Must(type => new[] { "rating", "checkbox_group", "dropdown", "comment" }.Contains(type.ToLower()))
            .WithMessage("Component type must be one of: rating, checkbox_group, dropdown, comment.");

        RuleFor(x => x.ComponentIndex)
            .GreaterThanOrEqualTo(0).WithMessage("Component index must be 0 or greater.");

        RuleFor(x => x.ResponseValue)
            .NotEmpty().WithMessage("Response value is required and cannot be empty.")
            .Must(BeValidJson).WithMessage("Response value must be valid JSON format.");

        RuleFor(x => x)
            .Must(HaveValidResponseValueForComponentType)
            .WithMessage("Response value format does not match the component type. Rating expects a number, checkbox_group expects an array, dropdown and comment expect a string.");
    }

    private bool BeValidJson(string? json)
    {
        if (string.IsNullOrWhiteSpace(json))
            return false;

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

    private bool HaveValidResponseValueForComponentType(SubmitQuestionResponseCommand command)
    {
        if (string.IsNullOrWhiteSpace(command.ResponseValue))
            return false;

        try
        {
            var doc = JsonDocument.Parse(command.ResponseValue);
            var root = doc.RootElement;

            return command.ComponentType.ToLower() switch
            {
                "rating" => root.ValueKind == JsonValueKind.Number,
                "checkbox_group" => root.ValueKind == JsonValueKind.Array,
                "dropdown" => root.ValueKind == JsonValueKind.String,
                "comment" => root.ValueKind == JsonValueKind.String,
                _ => false
            };
        }
        catch
        {
            return false;
        }
    }
}
