using MediatR;
using Wellbeing.Application.DTOs;

namespace Wellbeing.Application.Features.Questions.Queries.GetQuestionsByClientId;

public class GetQuestionsByClientIdQuery : IRequest<IEnumerable<QuestionDto>>
{
    public int ClientId { get; set; }
}
