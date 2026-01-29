using Microsoft.EntityFrameworkCore;
using Wellbeing.Domain.Entities;

namespace Wellbeing.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Clients> Clients { get; }
    DbSet<AspNetUsers> AspNetUsers { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
