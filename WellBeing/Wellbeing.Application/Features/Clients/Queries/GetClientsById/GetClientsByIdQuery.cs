using MediatR;
using Wellbeing.Application.DTOs;

namespace Wellbeing.Application.Features.Clients.Queries.GetClientsById;

public class GetClientsByIdQuery : IRequest<ClientsDto>
{
    public int Id { get; set; }
}
