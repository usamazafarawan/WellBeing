using MediatR;
using Wellbeing.Application.DTOs;

namespace Wellbeing.Application.Features.Surveys.Commands.CreateSurvey;

public class CreateSurveyCommand : IRequest<SurveyDto>
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int ClientsId { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}
