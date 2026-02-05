using MediatR;

namespace Wellbeing.Application.Features.Clients.Commands.DeleteClients;

public class DeleteClientsCommand : IRequest<bool>
{
    public int Id { get; set; }
}
