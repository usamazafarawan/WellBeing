using MediatR;
using Wellbeing.Application.DTOs;

namespace Wellbeing.Application.Features.Questions.Queries.GetAllQuestions;

public class GetAllQuestionsQuery : IRequest<IEnumerable<QuestionDto>>
{
    public int? SurveyId { get; set; }
    public int? ClientsId { get; set; }
}
