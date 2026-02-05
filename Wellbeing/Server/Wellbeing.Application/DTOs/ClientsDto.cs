using System.Text.Json;

namespace Wellbeing.Application.DTOs;

public class ClientsDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Domain { get; set; } = string.Empty;
    public string InstructionsText { get; set; } = string.Empty;
    public JsonElement ClientSettings { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ModifiedAt { get; set; }
    public int AspNetUsersCount { get; set; }
}
