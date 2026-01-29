using MediatR;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Wellbeing.Application.DTOs;
using Wellbeing.Application.Common.Interfaces;

namespace Wellbeing.Application.Features.Clients.Queries.GetAllClients;

public class GetAllClientsQueryHandler : IRequestHandler<GetAllClientsQuery, IEnumerable<ClientsDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllClientsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ClientsDto>> Handle(GetAllClientsQuery request, CancellationToken cancellationToken)
    {
        var clients = await _context.Clients
            .Include(c => c.AspNetUsers)
            .Where(x => !x.IsDeleted)
            .OrderBy(x => x.CreatedAt)
            .ToListAsync(cancellationToken);

        return clients.Select(clientsEntity =>
        {
            var dto = _mapper.Map<ClientsDto>(clientsEntity);
            dto.AspNetUsersCount = clientsEntity.AspNetUsers.Count(c => !c.IsDeleted);
            return dto;
        });
    }
}
