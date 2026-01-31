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
            .Where(x => !x.IsDeleted)
            .OrderBy(x => x.CreatedAt)
            .ToListAsync(cancellationToken);

        var clientIds = clients.Select(c => c.Id).ToList();
        var aspNetUsersCounts = await _context.AspNetUsers
            .Where(a => clientIds.Contains(a.ClientsId) && !a.IsDeleted)
            .GroupBy(a => a.ClientsId)
            .Select(g => new { ClientsId = g.Key, Count = g.Count() })
            .ToDictionaryAsync(x => x.ClientsId, x => x.Count, cancellationToken);

        return clients.Select(clientsEntity =>
        {
            var dto = _mapper.Map<ClientsDto>(clientsEntity);
            dto.AspNetUsersCount = aspNetUsersCounts.GetValueOrDefault(clientsEntity.Id, 0);
            return dto;
        });
    }
}
