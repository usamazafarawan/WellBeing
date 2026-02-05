using MediatR;
using Wellbeing.Application.DTOs;

namespace Wellbeing.Application.Features.Clients.Queries.GetAllClients;

public class GetAllClientsQuery : IRequest<IEnumerable<ClientsDto>>
{
}
