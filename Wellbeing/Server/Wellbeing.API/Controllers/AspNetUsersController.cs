using Microsoft.AspNetCore.Mvc;
using MediatR;
using Wellbeing.Application.DTOs;
using Wellbeing.Application.Common.Interfaces;
using Wellbeing.Application.Features.AspNetUsers.Commands.CreateAspNetUsers;
using Wellbeing.Application.Features.AspNetUsers.Commands.UpdateAspNetUsers;
using Wellbeing.Application.Features.AspNetUsers.Commands.DeleteAspNetUsers;
using Wellbeing.Application.Features.AspNetUsers.Queries.GetAllAspNetUsers;
using Wellbeing.Application.Features.AspNetUsers.Queries.GetAspNetUsersById;

namespace Wellbeing.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AspNetUsersController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILoggerService _logger;

    public AspNetUsersController(IMediator mediator, ILoggerService logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AspNetUsersDto>>> GetAllAspNetUsers([FromQuery] int? clientsId, CancellationToken cancellationToken)
    {
        if (clientsId.HasValue)
        {
            _logger.LogInformation("Getting all aspnetusers for clients ID: {ClientsId}", clientsId.Value);
        }
        else
        {
            _logger.LogInformation("Getting all aspnetusers");
        }
        
        var query = new GetAllAspNetUsersQuery { ClientsId = clientsId };
        var aspNetUsers = await _mediator.Send(query, cancellationToken);
        _logger.LogInformation("Retrieved {Count} aspnetusers", aspNetUsers.Count());
        return Ok(aspNetUsers);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AspNetUsersDto>> GetAspNetUsersById(Guid id, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting aspnetusers with ID: {AspNetUsersId}", id);
        var query = new GetAspNetUsersByIdQuery { Id = id };
        var aspNetUsers = await _mediator.Send(query, cancellationToken);
        return Ok(aspNetUsers);
    }

    [HttpPost]
    public async Task<ActionResult<AspNetUsersDto>> CreateAspNetUsers([FromBody] CreateAspNetUsersCommand command, CancellationToken cancellationToken)
    {
        var aspNetUsers = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetAspNetUsersById), new { id = aspNetUsers.Id }, aspNetUsers);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<AspNetUsersDto>> UpdateAspNetUsers(Guid id, [FromBody] UpdateAspNetUsersCommand command, CancellationToken cancellationToken)
    {
        if (id != command.Id)
        {
            return BadRequest("ID mismatch");
        }

        var aspNetUsers = await _mediator.Send(command, cancellationToken);
        return Ok(aspNetUsers);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAspNetUsers(Guid id, CancellationToken cancellationToken)
    {
        var command = new DeleteAspNetUsersCommand { Id = id };
        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }
}
