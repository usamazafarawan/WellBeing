using MediatR;
using Wellbeing.Application.DTOs;

namespace Wellbeing.Application.Features.Surveys.Queries.GetAllSurveys;

public class GetAllSurveysQuery : IRequest<IEnumerable<SurveyDto>>
{
    public int? ClientsId { get; set; }
    public bool? IsActive { get; set; }
}
