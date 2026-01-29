using MediatR;
using Wellbeing.Application.DTOs;

namespace Wellbeing.Application.Features.AspNetUsers.Commands.CreateAspNetUsers;

public class CreateAspNetUsersCommand : IRequest<AspNetUsersDto>
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public bool IsFirstLogin { get; set; } = true;
    public bool RequiresOtpVerification { get; set; } = false;
    public string? AuthMethod { get; set; }
    public int ClientsId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public string? LeadershipLevel { get; set; }
    public string? Tenant { get; set; }
}
