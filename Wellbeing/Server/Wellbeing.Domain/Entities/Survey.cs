using Wellbeing.Domain.Common;

namespace Wellbeing.Domain.Entities;

public class Survey : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int ClientsId { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    
    public virtual Clients Clients { get; set; } = null!;
    public virtual ICollection<Question> Questions { get; set; } = new List<Question>();
}
