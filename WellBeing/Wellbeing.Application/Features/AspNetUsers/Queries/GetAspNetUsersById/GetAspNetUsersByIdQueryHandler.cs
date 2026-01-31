using MediatR;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Wellbeing.Application.DTOs;
using Wellbeing.Application.Common.Interfaces;

namespace Wellbeing.Application.Features.AspNetUsers.Queries.GetAspNetUsersById;

public class GetAspNetUsersByIdQueryHandler : IRequestHandler<GetAspNetUsersByIdQuery, AspNetUsersDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAspNetUsersByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<AspNetUsersDto> Handle(GetAspNetUsersByIdQuery request, CancellationToken cancellationToken)
    {
        var aspNetUser = await _context.AspNetUsers
            .Include(c => c.Clients)
            .FirstOrDefaultAsync(x => x.Id == request.Id && !x.IsDeleted, cancellationToken);

        if (aspNetUser == null)
        {
            throw new KeyNotFoundException($"User with ID {request.Id} was not found or has been deleted.");
        }

        var aspNetUsersDto = _mapper.Map<AspNetUsersDto>(aspNetUser);
        aspNetUsersDto.ClientsName = aspNetUser.Clients?.Name ?? string.Empty;
        return aspNetUsersDto;
    }
}
