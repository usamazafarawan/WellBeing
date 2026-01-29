using Microsoft.AspNetCore.Mvc;
using MediatR;
using Wellbeing.Application.DTOs;
using Wellbeing.Application.Common.Interfaces;
using Wellbeing.Application.Features.Questions.Commands.CreateQuestion;
using Wellbeing.Application.Features.Questions.Commands.UpdateQuestion;
using Wellbeing.Application.Features.Questions.Commands.DeleteQuestion;
using Wellbeing.Application.Features.Questions.Queries.GetAllQuestions;
using Wellbeing.Application.Features.Questions.Queries.GetQuestionById;
using Wellbeing.Application.Features.Questions.Queries.GetQuestionsByDimensionId;
using Wellbeing.Application.Features.Questions.Queries.GetQuestionsBySubDimensionId;
using Wellbeing.Application.Features.Questions.Queries.GetQuestionsByClientId;

namespace Wellbeing.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class QuestionsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILoggerService _logger;

    public QuestionsController(IMediator mediator, ILoggerService logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<QuestionDto>>> GetAllQuestions(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting all questions");
        var questions = await _mediator.Send(new GetAllQuestionsQuery(), cancellationToken);
        _logger.LogInformation("Retrieved {Count} questions", questions.Count());
        return Ok(questions);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<QuestionDto>> GetQuestionById(int id, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting question with ID: {Id}", id);
        var query = new GetQuestionByIdQuery { Id = id };
        var question = await _mediator.Send(query, cancellationToken);
        return Ok(question);
    }

    [HttpGet("dimension/{dimensionId}")]
    public async Task<ActionResult<IEnumerable<QuestionDto>>> GetQuestionsByDimensionId(int dimensionId, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting questions for dimension ID: {DimensionId}", dimensionId);
        var query = new GetQuestionsByDimensionIdQuery { DimensionId = dimensionId };
        var questions = await _mediator.Send(query, cancellationToken);
        _logger.LogInformation("Retrieved {Count} questions for dimension {DimensionId}", questions.Count(), dimensionId);
        return Ok(questions);
    }

    [HttpGet("subdimension/{subDimensionId}")]
    public async Task<ActionResult<IEnumerable<QuestionDto>>> GetQuestionsBySubDimensionId(int subDimensionId, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting questions for sub-dimension ID: {SubDimensionId}", subDimensionId);
        var query = new GetQuestionsBySubDimensionIdQuery { SubDimensionId = subDimensionId };
        var questions = await _mediator.Send(query, cancellationToken);
        _logger.LogInformation("Retrieved {Count} questions for sub-dimension {SubDimensionId}", questions.Count(), subDimensionId);
        return Ok(questions);
    }

    [HttpGet("client/{clientId}")]
    public async Task<ActionResult<IEnumerable<QuestionDto>>> GetQuestionsByClientId(int clientId, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting questions for client ID: {ClientId}", clientId);
        var query = new GetQuestionsByClientIdQuery { ClientId = clientId };
        var questions = await _mediator.Send(query, cancellationToken);
        _logger.LogInformation("Retrieved {Count} questions for client {ClientId}", questions.Count(), clientId);
        return Ok(questions);
    }

    [HttpPost]
    public async Task<ActionResult<QuestionDto>> CreateQuestion([FromBody] CreateQuestionCommand command, CancellationToken cancellationToken)
    {
        var question = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetQuestionById), new { id = question.Id }, question);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<QuestionDto>> UpdateQuestion(int id, [FromBody] UpdateQuestionCommand command, CancellationToken cancellationToken)
    {
        if (id != command.Id)
        {
            return BadRequest("ID mismatch");
        }

        var question = await _mediator.Send(command, cancellationToken);
        return Ok(question);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteQuestion(int id, CancellationToken cancellationToken)
    {
        var command = new DeleteQuestionCommand { Id = id };
        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }
}
