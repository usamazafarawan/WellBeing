using Microsoft.AspNetCore.Mvc;
using MediatR;
using Wellbeing.Application.DTOs;
using Wellbeing.Application.Common.Interfaces;
using Wellbeing.Application.Features.WellbeingSubDimensions.Commands.CreateWellbeingSubDimension;
using Wellbeing.Application.Features.WellbeingSubDimensions.Commands.UpdateWellbeingSubDimension;
using Wellbeing.Application.Features.WellbeingSubDimensions.Commands.DeleteWellbeingSubDimension;
using Wellbeing.Application.Features.WellbeingSubDimensions.Queries.GetAllWellbeingSubDimensions;
using Wellbeing.Application.Features.WellbeingSubDimensions.Queries.GetWellbeingSubDimensionById;
using Wellbeing.Application.Features.WellbeingSubDimensions.Queries.GetWellbeingSubDimensionsByDimensionId;
using Wellbeing.Application.Features.WellbeingSubDimensions.Queries.GetWellbeingSubDimensionsByClientId;

namespace Wellbeing.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WellbeingSubDimensionsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILoggerService _logger;

    public WellbeingSubDimensionsController(IMediator mediator, ILoggerService logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<WellbeingSubDimensionDto>>> GetAllWellbeingSubDimensions(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting all wellbeing sub-dimensions");
        var subDimensions = await _mediator.Send(new GetAllWellbeingSubDimensionsQuery(), cancellationToken);
        _logger.LogInformation("Retrieved {Count} wellbeing sub-dimensions", subDimensions.Count());
        return Ok(subDimensions);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<WellbeingSubDimensionDto>> GetWellbeingSubDimensionById(int id, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting wellbeing sub-dimension with ID: {Id}", id);
        var query = new GetWellbeingSubDimensionByIdQuery { Id = id };
        var subDimension = await _mediator.Send(query, cancellationToken);
        return Ok(subDimension);
    }

    [HttpGet("dimension/{dimensionId}")]
    public async Task<ActionResult<IEnumerable<WellbeingSubDimensionDto>>> GetWellbeingSubDimensionsByDimensionId(int dimensionId, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting wellbeing sub-dimensions for dimension ID: {DimensionId}", dimensionId);
        var query = new GetWellbeingSubDimensionsByDimensionIdQuery { DimensionId = dimensionId };
        var subDimensions = await _mediator.Send(query, cancellationToken);
        _logger.LogInformation("Retrieved {Count} wellbeing sub-dimensions for dimension {DimensionId}", subDimensions.Count(), dimensionId);
        return Ok(subDimensions);
    }

    [HttpGet("client/{clientId}")]
    public async Task<ActionResult<IEnumerable<WellbeingSubDimensionDto>>> GetWellbeingSubDimensionsByClientId(int clientId, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting wellbeing sub-dimensions for client ID: {ClientId}", clientId);
        var query = new GetWellbeingSubDimensionsByClientIdQuery { ClientId = clientId };
        var subDimensions = await _mediator.Send(query, cancellationToken);
        _logger.LogInformation("Retrieved {Count} wellbeing sub-dimensions for client {ClientId}", subDimensions.Count(), clientId);
        return Ok(subDimensions);
    }

    [HttpPost]
    public async Task<ActionResult<WellbeingSubDimensionDto>> CreateWellbeingSubDimension([FromBody] CreateWellbeingSubDimensionCommand command, CancellationToken cancellationToken)
    {
        var subDimension = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetWellbeingSubDimensionById), new { id = subDimension.Id }, subDimension);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<WellbeingSubDimensionDto>> UpdateWellbeingSubDimension(int id, [FromBody] UpdateWellbeingSubDimensionCommand command, CancellationToken cancellationToken)
    {
        if (id != command.Id)
        {
            return BadRequest("ID mismatch");
        }

        var subDimension = await _mediator.Send(command, cancellationToken);
        return Ok(subDimension);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteWellbeingSubDimension(int id, CancellationToken cancellationToken)
    {
        var command = new DeleteWellbeingSubDimensionCommand { Id = id };
        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }
}
