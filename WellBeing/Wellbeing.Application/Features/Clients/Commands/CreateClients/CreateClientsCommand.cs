using MediatR;
using Wellbeing.Application.DTOs;

namespace Wellbeing.Application.Features.Clients.Commands.CreateClients;

public class CreateClientsCommand : IRequest<ClientsDto>
{
    public string Name { get; set; } = string.Empty;
    public string Domain { get; set; } = string.Empty;
    public string InstructionsText { get; set; } = string.Empty;
    public string? ClientSettings { get; set; }
}
