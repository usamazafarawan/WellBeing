namespace Wellbeing.Application.DTOs;

public class AspNetUsersDto
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public bool IsFirstLogin { get; set; }
    public bool RequiresOtpVerification { get; set; }
    public string? AuthMethod { get; set; }
    public int ClientId { get; set; }
    public string ClientsName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? ModifiedAt { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string NormalizedUserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string NormalizedEmail { get; set; } = string.Empty;
    public bool EmailConfirmed { get; set; }
    public string PasswordHash { get; set; } = string.Empty;
    public string SecurityStamp { get; set; } = string.Empty;
    public string ConcurrencyStamp { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public bool PhoneNumberConfirmed { get; set; }
    public bool TwoFactorEnabled { get; set; }
    public DateTime? LockoutEnd { get; set; }
    public bool LockoutEnabled { get; set; }
    public int AccessFailedCount { get; set; }
    public string? LeadershipLevel { get; set; }
    public string? Tenant { get; set; }
}
