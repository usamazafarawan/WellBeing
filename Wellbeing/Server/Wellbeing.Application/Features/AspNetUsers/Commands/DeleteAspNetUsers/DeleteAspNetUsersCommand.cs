using MediatR;

namespace Wellbeing.Application.Features.AspNetUsers.Commands.DeleteAspNetUsers;

public class DeleteAspNetUsersCommand : IRequest<bool>
{
    public Guid Id { get; set; }
}
