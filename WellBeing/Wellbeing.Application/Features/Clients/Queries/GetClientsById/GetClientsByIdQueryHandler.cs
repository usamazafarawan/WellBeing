using MediatR;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Wellbeing.Application.DTOs;
using Wellbeing.Application.Common.Interfaces;

namespace Wellbeing.Application.Features.Clients.Queries.GetClientsById;

public class GetClientsByIdQueryHandler : IRequestHandler<GetClientsByIdQuery, ClientsDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetClientsByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ClientsDto> Handle(GetClientsByIdQuery request, CancellationToken cancellationToken)
    {
        var clients = await _context.Clients
            .Include(c => c.AspNetUsers)
            .FirstOrDefaultAsync(x => x.Id == request.Id && !x.IsDeleted, cancellationToken);

        if (clients == null)
        {
            throw new KeyNotFoundException($"Clients with ID {request.Id} was not found.");
        }

        var clientsDto = _mapper.Map<ClientsDto>(clients);
        clientsDto.AspNetUsersCount = clients.AspNetUsers.Count(c => !c.IsDeleted);
        return clientsDto;
    }
}
