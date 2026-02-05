namespace Wellbeing.Application.DTOs;

public class WellbeingDimensionDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int ClientsId { get; set; }
    public string? ClientsName { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ModifiedAt { get; set; }
}
