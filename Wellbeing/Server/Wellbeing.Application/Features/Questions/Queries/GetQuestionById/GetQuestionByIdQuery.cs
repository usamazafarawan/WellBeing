using MediatR;
using Wellbeing.Application.DTOs;

namespace Wellbeing.Application.Features.Questions.Queries.GetQuestionById;

public class GetQuestionByIdQuery : IRequest<QuestionDto>
{
    public int Id { get; set; }
}
