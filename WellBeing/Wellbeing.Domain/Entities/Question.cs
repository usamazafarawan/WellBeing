using Wellbeing.Domain.Common;
using System.Text.Json;

namespace Wellbeing.Domain.Entities;

public class Question : BaseEntity
{
    public string QuestionText { get; set; } = string.Empty;
    public string? QuestionType { get; set; }
    public int SurveyId { get; set; }
    public int ClientsId { get; set; }
    public string? QuestionConfig { get; set; } // JSONB - stores component structure
    public bool IsRequired { get; set; } = true;
    public int DisplayOrder { get; set; } = 0;
    
    // Optional: Keep for backward compatibility or reporting
    public int? WellbeingDimensionId { get; set; }
    public int? WellbeingSubDimensionId { get; set; }
    
    public virtual Survey Survey { get; set; } = null!;
    public virtual Clients Clients { get; set; } = null!;
    public virtual WellbeingDimension? WellbeingDimension { get; set; }
    public virtual WellbeingSubDimension? WellbeingSubDimension { get; set; }
}
