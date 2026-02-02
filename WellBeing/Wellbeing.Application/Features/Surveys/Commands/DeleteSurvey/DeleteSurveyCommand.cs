using MediatR;

namespace Wellbeing.Application.Features.Surveys.Commands.DeleteSurvey;

public class DeleteSurveyCommand : IRequest<Unit>
{
    public int Id { get; set; }
}
