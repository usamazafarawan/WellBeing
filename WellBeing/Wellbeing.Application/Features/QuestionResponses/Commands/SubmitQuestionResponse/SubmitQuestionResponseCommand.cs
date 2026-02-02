using MediatR;
using Wellbeing.Application.DTOs;

namespace Wellbeing.Application.Features.QuestionResponses.Commands.SubmitQuestionResponse;

public class SubmitQuestionResponseCommand : IRequest<QuestionResponseDto>
{
    public int QuestionId { get; set; }
    public Guid AspNetUsersId { get; set; }
    public int ClientsId { get; set; }
    public string ComponentType { get; set; } = string.Empty; // rating, checkbox_group, dropdown, comment
    public int ComponentIndex { get; set; } = 0;
    public string ResponseValue { get; set; } = string.Empty; // JSON string
}
