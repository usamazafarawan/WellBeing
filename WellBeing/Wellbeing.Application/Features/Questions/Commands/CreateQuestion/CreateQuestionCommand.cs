using MediatR;
using Wellbeing.Application.DTOs;

namespace Wellbeing.Application.Features.Questions.Commands.CreateQuestion;

public class CreateQuestionCommand : IRequest<QuestionDto>
{
    public string QuestionText { get; set; } = string.Empty;
    public string? QuestionType { get; set; }
    public int WellbeingDimensionId { get; set; }
    public int WellbeingSubDimensionId { get; set; }
    public int ClientsId { get; set; }
}
