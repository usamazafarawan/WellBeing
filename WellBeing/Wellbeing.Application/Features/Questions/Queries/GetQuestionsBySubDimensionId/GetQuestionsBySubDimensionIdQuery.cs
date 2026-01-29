using MediatR;
using Wellbeing.Application.DTOs;

namespace Wellbeing.Application.Features.Questions.Queries.GetQuestionsBySubDimensionId;

public class GetQuestionsBySubDimensionIdQuery : IRequest<IEnumerable<QuestionDto>>
{
    public int SubDimensionId { get; set; }
}
