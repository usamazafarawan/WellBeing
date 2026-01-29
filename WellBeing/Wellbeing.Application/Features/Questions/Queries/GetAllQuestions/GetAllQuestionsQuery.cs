using MediatR;
using Wellbeing.Application.DTOs;

namespace Wellbeing.Application.Features.Questions.Queries.GetAllQuestions;

public class GetAllQuestionsQuery : IRequest<IEnumerable<QuestionDto>>
{
}
