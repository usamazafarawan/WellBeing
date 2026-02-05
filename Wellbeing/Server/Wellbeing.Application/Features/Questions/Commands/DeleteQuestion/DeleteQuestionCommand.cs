using MediatR;

namespace Wellbeing.Application.Features.Questions.Commands.DeleteQuestion;

public class DeleteQuestionCommand : IRequest<Unit>
{
    public int Id { get; set; }
}
