using Wellbeing.Domain.Common;

namespace Wellbeing.Domain.Entities;

public class QuestionResponse : BaseEntity
{
    public int QuestionId { get; set; }
    public Guid AspNetUsersId { get; set; }
    public int ClientsId { get; set; }
    public string ComponentType { get; set; } = string.Empty; // rating, checkbox_group, dropdown, comment
    public int ComponentIndex { get; set; } = 0; // Index of component within question
    public string ResponseValue { get; set; } = string.Empty; // JSON string - stores the response value (stored as JSONB in DB)
    
    public virtual Question Question { get; set; } = null!;
    public virtual AspNetUsers AspNetUsers { get; set; } = null!;
    public virtual Clients Clients { get; set; } = null!;
}
