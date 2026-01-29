using MediatR;
using Wellbeing.Application.DTOs;

namespace Wellbeing.Application.Features.Questions.Commands.UpdateQuestion;

public class UpdateQuestionCommand : IRequest<QuestionDto>
{
    public int Id { get; set; }
    public string QuestionText { get; set; } = string.Empty;
    public string? QuestionType { get; set; }
    public int WellbeingDimensionId { get; set; }
    public int WellbeingSubDimensionId { get; set; }
    public int ClientsId { get; set; }
}
