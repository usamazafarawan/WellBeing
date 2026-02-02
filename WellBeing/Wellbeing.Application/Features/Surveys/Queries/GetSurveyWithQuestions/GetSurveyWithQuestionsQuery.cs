using MediatR;
using Wellbeing.Application.DTOs;

namespace Wellbeing.Application.Features.Surveys.Queries.GetSurveyWithQuestions;

public class GetSurveyWithQuestionsQuery : IRequest<SurveyWithQuestionsDto>
{
    public int Id { get; set; }
}

public class SurveyWithQuestionsDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int ClientsId { get; set; }
    public string? ClientsName { get; set; }
    public bool IsActive { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public List<QuestionDto> Questions { get; set; } = new();
    public DateTime CreatedAt { get; set; }
    public DateTime? ModifiedAt { get; set; }
}
