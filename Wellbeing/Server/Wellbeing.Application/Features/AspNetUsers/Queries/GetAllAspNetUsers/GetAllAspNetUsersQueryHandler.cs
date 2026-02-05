using MediatR;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Wellbeing.Application.DTOs;
using Wellbeing.Application.Common.Interfaces;

namespace Wellbeing.Application.Features.AspNetUsers.Queries.GetAllAspNetUsers;

public class GetAllAspNetUsersQueryHandler : IRequestHandler<GetAllAspNetUsersQuery, IEnumerable<AspNetUsersDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllAspNetUsersQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<AspNetUsersDto>> Handle(GetAllAspNetUsersQuery request, CancellationToken cancellationToken)
    {
        var query = _context.AspNetUsers
            .Include(c => c.Clients)
            .Where(x => !x.IsDeleted);

        if (request.ClientsId.HasValue)
        {
            query = query.Where(c => c.ClientsId == request.ClientsId.Value);
        }

        var aspNetUsers = await query
            .OrderBy(x => x.CreatedAt)
            .ToListAsync(cancellationToken);

        return aspNetUsers.Select(aspNetUser =>
        {
            var dto = _mapper.Map<AspNetUsersDto>(aspNetUser);
            dto.ClientsName = aspNetUser.Clients?.Name ?? string.Empty;
            return dto;
        });
    }
}
