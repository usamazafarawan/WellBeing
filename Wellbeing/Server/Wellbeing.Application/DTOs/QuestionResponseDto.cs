using System.Text.Json;

namespace Wellbeing.Application.DTOs;

public class QuestionResponseDto
{
    public int Id { get; set; }
    public int QuestionId { get; set; }
    public string? QuestionText { get; set; }
    public Guid AspNetUsersId { get; set; }
    public string? UserName { get; set; }
    public int ClientsId { get; set; }
    public string ComponentType { get; set; } = string.Empty;
    public int ComponentIndex { get; set; }
    public JsonElement ResponseValue { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ModifiedAt { get; set; }
}
