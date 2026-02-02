using MediatR;
using Wellbeing.Application.DTOs;

namespace Wellbeing.Application.Features.QuestionResponses.Queries.GetQuestionResponses;

public class GetQuestionResponsesQuery : IRequest<IEnumerable<QuestionResponseDto>>
{
    public int? QuestionId { get; set; }
    public Guid? AspNetUsersId { get; set; }
    public int? ClientsId { get; set; }
}
