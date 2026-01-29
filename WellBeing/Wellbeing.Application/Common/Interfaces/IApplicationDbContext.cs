using Microsoft.EntityFrameworkCore;
using Wellbeing.Domain.Entities;

namespace Wellbeing.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Clients> Clients { get; }
    DbSet<AspNetUsers> AspNetUsers { get; }
    DbSet<WellbeingDimension> WellbeingDimensions { get; }
    DbSet<WellbeingSubDimension> WellbeingSubDimensions { get; }
    DbSet<Question> Questions { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
