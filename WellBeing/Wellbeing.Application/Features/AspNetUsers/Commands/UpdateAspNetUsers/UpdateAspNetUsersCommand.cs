using MediatR;
using Wellbeing.Application.DTOs;

namespace Wellbeing.Application.Features.AspNetUsers.Commands.UpdateAspNetUsers;

public class UpdateAspNetUsersCommand : IRequest<AspNetUsersDto>
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public bool IsFirstLogin { get; set; }
    public bool RequiresOtpVerification { get; set; }
    public string? AuthMethod { get; set; }
    public int ClientsId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? PasswordHash { get; set; }
    public string? PhoneNumber { get; set; }
    public bool EmailConfirmed { get; set; }
    public bool PhoneNumberConfirmed { get; set; }
    public bool TwoFactorEnabled { get; set; }
    public DateTime? LockoutEnd { get; set; }
    public bool LockoutEnabled { get; set; }
    public string? LeadershipLevel { get; set; }
    public string? Tenant { get; set; }
}
