using Microsoft.AspNetCore.Mvc;
using MediatR;
using Wellbeing.Application.DTOs;
using Wellbeing.Application.Common.Interfaces;
using Wellbeing.Application.Features.WellbeingDimensions.Commands.CreateWellbeingDimension;
using Wellbeing.Application.Features.WellbeingDimensions.Commands.UpdateWellbeingDimension;
using Wellbeing.Application.Features.WellbeingDimensions.Commands.DeleteWellbeingDimension;
using Wellbeing.Application.Features.WellbeingDimensions.Queries.GetAllWellbeingDimensions;
using Wellbeing.Application.Features.WellbeingDimensions.Queries.GetWellbeingDimensionById;
using Wellbeing.Application.Features.WellbeingDimensions.Queries.GetWellbeingDimensionsByClientId;

namespace Wellbeing.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WellbeingDimensionsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILoggerService _logger;

    public WellbeingDimensionsController(IMediator mediator, ILoggerService logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<WellbeingDimensionDto>>> GetAllWellbeingDimensions(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting all wellbeing dimensions");
        var dimensions = await _mediator.Send(new GetAllWellbeingDimensionsQuery(), cancellationToken);
        _logger.LogInformation("Retrieved {Count} wellbeing dimensions", dimensions.Count());
        return Ok(dimensions);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<WellbeingDimensionDto>> GetWellbeingDimensionById(int id, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting wellbeing dimension with ID: {Id}", id);
        var query = new GetWellbeingDimensionByIdQuery { Id = id };
        var dimension = await _mediator.Send(query, cancellationToken);
        return Ok(dimension);
    }

    [HttpGet("client/{clientId}")]
    public async Task<ActionResult<IEnumerable<WellbeingDimensionDto>>> GetWellbeingDimensionsByClientId(int clientId, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting wellbeing dimensions for client ID: {ClientId}", clientId);
        var query = new GetWellbeingDimensionsByClientIdQuery { ClientId = clientId };
        var dimensions = await _mediator.Send(query, cancellationToken);
        _logger.LogInformation("Retrieved {Count} wellbeing dimensions for client {ClientId}", dimensions.Count(), clientId);
        return Ok(dimensions);
    }

    [HttpPost]
    public async Task<ActionResult<WellbeingDimensionDto>> CreateWellbeingDimension([FromBody] CreateWellbeingDimensionCommand command, CancellationToken cancellationToken)
    {
        var dimension = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetWellbeingDimensionById), new { id = dimension.Id }, dimension);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<WellbeingDimensionDto>> UpdateWellbeingDimension(int id, [FromBody] UpdateWellbeingDimensionCommand command, CancellationToken cancellationToken)
    {
        if (id != command.Id)
        {
            return BadRequest("ID mismatch");
        }

        var dimension = await _mediator.Send(command, cancellationToken);
        return Ok(dimension);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteWellbeingDimension(int id, CancellationToken cancellationToken)
    {
        var command = new DeleteWellbeingDimensionCommand { Id = id };
        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }
}
