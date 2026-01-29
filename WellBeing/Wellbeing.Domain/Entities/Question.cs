using Wellbeing.Domain.Common;

namespace Wellbeing.Domain.Entities;

public class Question : BaseEntity
{
    public string QuestionText { get; set; } = string.Empty;
    public string? QuestionType { get; set; }
    public int WellbeingDimensionId { get; set; }
    public int WellbeingSubDimensionId { get; set; }
    public int ClientsId { get; set; }
    
    public virtual WellbeingDimension WellbeingDimension { get; set; } = null!;
    public virtual WellbeingSubDimension WellbeingSubDimension { get; set; } = null!;
    public virtual Clients Clients { get; set; } = null!;
}
