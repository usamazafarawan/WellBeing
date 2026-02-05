using MediatR;
using Wellbeing.Application.DTOs;

namespace Wellbeing.Application.Features.Questions.Commands.UpdateQuestion;

public class UpdateQuestionCommand : IRequest<QuestionDto>
{
    public int Id { get; set; }
    public string QuestionText { get; set; } = string.Empty;
    public string? QuestionType { get; set; }
    public int SurveyId { get; set; }
    public string? QuestionConfig { get; set; } // JSON string for component structure
    public bool IsRequired { get; set; }
    public int DisplayOrder { get; set; }
    public int? WellbeingDimensionId { get; set; } // Optional
    public int? WellbeingSubDimensionId { get; set; } // Optional
}
