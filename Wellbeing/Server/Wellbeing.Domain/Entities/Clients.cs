using Wellbeing.Domain.Common;

namespace Wellbeing.Domain.Entities;

public class Clients : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Domain { get; set; } = string.Empty;
    public string InstructionsText { get; set; } = string.Empty;
    public string ClientSettings { get; set; } = string.Empty;

    public virtual ICollection<AspNetUsers> AspNetUsers { get; set; } = new List<AspNetUsers>();
    public virtual ICollection<WellbeingDimension> WellbeingDimensions { get; set; } = new List<WellbeingDimension>();
    public virtual ICollection<Survey> Surveys { get; set; } = new List<Survey>();
}
