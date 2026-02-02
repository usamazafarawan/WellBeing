using Microsoft.AspNetCore.Mvc;
using MediatR;
using Wellbeing.Application.DTOs;
using Wellbeing.Application.Common.Interfaces;
using Wellbeing.Application.Features.QuestionResponses.Commands.SubmitQuestionResponse;
using Wellbeing.Application.Features.QuestionResponses.Queries.GetQuestionResponses;

namespace Wellbeing.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class QuestionResponsesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILoggerService _logger;

    public QuestionResponsesController(IMediator mediator, ILoggerService logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<QuestionResponseDto>>> GetQuestionResponses(
        [FromQuery] int? questionId,
        [FromQuery] Guid? aspNetUsersId,
        [FromQuery] int? clientsId,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting question responses");
        var query = new GetQuestionResponsesQuery 
        { 
            QuestionId = questionId, 
            AspNetUsersId = aspNetUsersId, 
            ClientsId = clientsId 
        };
        var responses = await _mediator.Send(query, cancellationToken);
        return Ok(responses);
    }

    [HttpPost]
    public async Task<ActionResult<QuestionResponseDto>> SubmitQuestionResponse(
        [FromBody] SubmitQuestionResponseCommand command, 
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetQuestionResponses), new { questionId = response.QuestionId }, response);
    }
}
