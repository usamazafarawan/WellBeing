using MediatR;
using Wellbeing.Application.DTOs;

namespace Wellbeing.Application.Features.Questions.Queries.GetQuestionsByDimensionId;

public class GetQuestionsByDimensionIdQuery : IRequest<IEnumerable<QuestionDto>>
{
    public int DimensionId { get; set; }
}
