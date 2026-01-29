using MediatR;
using Wellbeing.Application.DTOs;

namespace Wellbeing.Application.Features.Clients.Commands.UpdateClients;

public class UpdateClientsCommand : IRequest<ClientsDto>
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Domain { get; set; } = string.Empty;
    public string InstructionsText { get; set; } = string.Empty;
    public string? ClientSettings { get; set; }
}
