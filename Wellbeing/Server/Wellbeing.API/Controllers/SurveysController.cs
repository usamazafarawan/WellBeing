using Microsoft.AspNetCore.Mvc;
using MediatR;
using Wellbeing.Application.DTOs;
using Wellbeing.Application.Common.Interfaces;
using Wellbeing.Application.Features.Surveys.Commands.CreateSurvey;
using Wellbeing.Application.Features.Surveys.Commands.UpdateSurvey;
using Wellbeing.Application.Features.Surveys.Commands.DeleteSurvey;
using Wellbeing.Application.Features.Surveys.Queries.GetAllSurveys;
using Wellbeing.Application.Features.Surveys.Queries.GetSurveyById;
using Wellbeing.Application.Features.Surveys.Queries.GetSurveyWithQuestions;

namespace Wellbeing.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SurveysController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILoggerService _logger;

    public SurveysController(IMediator mediator, ILoggerService logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<SurveyDto>>> GetAllSurveys(
        [FromQuery] int? clientsId,
        [FromQuery] bool? isActive,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting all surveys");
        var query = new GetAllSurveysQuery { ClientsId = clientsId, IsActive = isActive };
        var surveys = await _mediator.Send(query, cancellationToken);
        _logger.LogInformation("Retrieved {Count} surveys", surveys.Count());
        return Ok(surveys);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<SurveyDto>> GetSurveyById(int id, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting survey with ID: {SurveyId}", id);
        var query = new GetSurveyByIdQuery { Id = id };
        var survey = await _mediator.Send(query, cancellationToken);
        return Ok(survey);
    }

    [HttpGet("{id}/questions")]
    public async Task<ActionResult<SurveyWithQuestionsDto>> GetSurveyWithQuestions(int id, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting survey with questions for ID: {SurveyId}", id);
        var query = new GetSurveyWithQuestionsQuery { Id = id };
        var survey = await _mediator.Send(query, cancellationToken);
        return Ok(survey);
    }

    [HttpPost]
    public async Task<ActionResult<SurveyDto>> CreateSurvey([FromBody] CreateSurveyCommand command, CancellationToken cancellationToken)
    {
        var survey = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetSurveyById), new { id = survey.Id }, survey);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<SurveyDto>> UpdateSurvey(int id, [FromBody] UpdateSurveyCommand command, CancellationToken cancellationToken)
    {
        if (id != command.Id)
        {
            return BadRequest("ID mismatch");
        }

        var survey = await _mediator.Send(command, cancellationToken);
        return Ok(survey);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSurvey(int id, CancellationToken cancellationToken)
    {
        var command = new DeleteSurveyCommand { Id = id };
        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }
}
