using MediatR;
using Wellbeing.Application.DTOs;

namespace Wellbeing.Application.Features.AspNetUsers.Queries.GetAspNetUsersById;

public class GetAspNetUsersByIdQuery : IRequest<AspNetUsersDto>
{
    public Guid Id { get; set; }
}
