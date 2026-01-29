namespace Wellbeing.Application.DTOs;

public class QuestionDto
{
    public int Id { get; set; }
    public string QuestionText { get; set; } = string.Empty;
    public string? QuestionType { get; set; }
    public int WellbeingDimensionId { get; set; }
    public string? WellbeingDimensionName { get; set; }
    public int WellbeingSubDimensionId { get; set; }
    public string? WellbeingSubDimensionName { get; set; }
    public int ClientsId { get; set; }
    public string? ClientsName { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ModifiedAt { get; set; }
}
