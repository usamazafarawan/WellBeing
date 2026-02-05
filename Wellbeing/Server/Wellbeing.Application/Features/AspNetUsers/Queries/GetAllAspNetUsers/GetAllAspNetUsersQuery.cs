using MediatR;
using Wellbeing.Application.DTOs;

namespace Wellbeing.Application.Features.AspNetUsers.Queries.GetAllAspNetUsers;

public class GetAllAspNetUsersQuery : IRequest<IEnumerable<AspNetUsersDto>>
{
    public int? ClientsId { get; set; }
}
