using Microsoft.AspNetCore.Mvc;
using MediatR;
using Wellbeing.Application.DTOs;
using Wellbeing.Application.Common.Interfaces;
using Wellbeing.Application.Features.Clients.Commands.CreateClients;
using Wellbeing.Application.Features.Clients.Commands.UpdateClients;
using Wellbeing.Application.Features.Clients.Commands.DeleteClients;
using Wellbeing.Application.Features.Clients.Queries.GetAllClients;
using Wellbeing.Application.Features.Clients.Queries.GetClientsById;

namespace Wellbeing.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClientsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILoggerService _logger;

    public ClientsController(IMediator mediator, ILoggerService logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ClientsDto>>> GetAllClients(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting all clients");
        var clients = await _mediator.Send(new GetAllClientsQuery(), cancellationToken);
        _logger.LogInformation("Retrieved {Count} clients", clients.Count());
        return Ok(clients);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ClientsDto>> GetClientsById(int id, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting clients with ID: {ClientsId}", id);
        var query = new GetClientsByIdQuery { Id = id };
        var clients = await _mediator.Send(query, cancellationToken);
        return Ok(clients);
    }

    [HttpPost]
    public async Task<ActionResult<ClientsDto>> CreateClients([FromBody] CreateClientsCommand command, CancellationToken cancellationToken)
    {
        var clients = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetClientsById), new { id = clients.Id }, clients);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ClientsDto>> UpdateClients(int id, [FromBody] UpdateClientsCommand command, CancellationToken cancellationToken)
    {
        if (id != command.Id)
        {
            return BadRequest("ID mismatch");
        }

        var clients = await _mediator.Send(command, cancellationToken);
        return Ok(clients);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteClients(int id, CancellationToken cancellationToken)
    {
        var command = new DeleteClientsCommand { Id = id };
        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }
}
