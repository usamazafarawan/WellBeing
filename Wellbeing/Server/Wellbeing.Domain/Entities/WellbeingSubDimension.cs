using Wellbeing.Domain.Common;

namespace Wellbeing.Domain.Entities;

public class WellbeingSubDimension : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int WellbeingDimensionId { get; set; }
    public int ClientsId { get; set; }
    
    public virtual WellbeingDimension WellbeingDimension { get; set; } = null!;
    public virtual Clients Clients { get; set; } = null!;
    public virtual ICollection<Question> Questions { get; set; } = new List<Question>();
}
