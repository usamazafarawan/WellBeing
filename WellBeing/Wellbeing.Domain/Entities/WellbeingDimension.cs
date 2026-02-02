using Wellbeing.Domain.Common;

namespace Wellbeing.Domain.Entities;

public class WellbeingDimension : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int ClientsId { get; set; }
    
    public virtual Clients Clients { get; set; } = null!;
    public virtual ICollection<WellbeingSubDimension> WellbeingSubDimensions { get; set; } = new List<WellbeingSubDimension>();
    public virtual ICollection<Question> Questions { get; set; } = new List<Question>(); // Optional - for backward compatibility
}
