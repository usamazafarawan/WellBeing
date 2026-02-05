using MediatR;
using Wellbeing.Application.DTOs;

namespace Wellbeing.Application.Features.Surveys.Queries.GetSurveyById;

public class GetSurveyByIdQuery : IRequest<SurveyDto>
{
    public int Id { get; set; }
}
