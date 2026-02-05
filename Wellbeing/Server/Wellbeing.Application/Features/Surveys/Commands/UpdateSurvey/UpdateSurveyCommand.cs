using MediatR;
using Wellbeing.Application.DTOs;

namespace Wellbeing.Application.Features.Surveys.Commands.UpdateSurvey;

public class UpdateSurveyCommand : IRequest<SurveyDto>
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}
