using MediatR;
using Wellbeing.Application.DTOs;

namespace Wellbeing.Application.Features.Questions.Commands.CreateQuestion;

public class CreateQuestionCommand : IRequest<QuestionDto>
{
    public string QuestionText { get; set; } = string.Empty;
    public string? QuestionType { get; set; }
    public int SurveyId { get; set; }
    public int ClientsId { get; set; }
    public string? QuestionConfig { get; set; } // JSON string for component structure
    public bool IsRequired { get; set; } = true;
    public int DisplayOrder { get; set; } = 0;
    public int? WellbeingDimensionId { get; set; } // Optional
    public int? WellbeingSubDimensionId { get; set; } // Optional
}
