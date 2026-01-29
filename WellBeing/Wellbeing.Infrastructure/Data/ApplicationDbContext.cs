using Microsoft.EntityFrameworkCore;
using Wellbeing.Application.Common.Interfaces;
using Wellbeing.Domain.Entities;
using Wellbeing.Domain.Common;

namespace Wellbeing.Infrastructure.Data;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Clients> Clients { get; set; } = null!;
    public DbSet<AspNetUsers> AspNetUsers { get; set; } = null!;
    public DbSet<WellbeingDimension> WellbeingDimensions { get; set; } = null!;
    public DbSet<WellbeingSubDimension> WellbeingSubDimensions { get; set; } = null!;
    public DbSet<Question> Questions { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Clients>(entity =>
        {
            entity.ToTable("Clients");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Domain).IsRequired().HasMaxLength(255);
            entity.Property(e => e.InstructionsText).IsRequired().HasMaxLength(5000);
            entity.Property(e => e.ClientSettings).IsRequired().HasColumnType("jsonb");
            entity.HasIndex(e => e.Domain).IsUnique();
            entity.HasQueryFilter(e => !e.IsDeleted);
        });

        modelBuilder.Entity<AspNetUsers>(entity =>
        {
            entity.ToTable("AspNetUsers");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.LastName).HasMaxLength(100);
            entity.Property(e => e.UserName).IsRequired().HasMaxLength(256);
            entity.Property(e => e.NormalizedUserName).IsRequired().HasMaxLength(256);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(256);
            entity.Property(e => e.NormalizedEmail).IsRequired().HasMaxLength(256);
            entity.Property(e => e.PasswordHash).IsRequired();
            entity.Property(e => e.SecurityStamp).IsRequired();
            entity.Property(e => e.ConcurrencyStamp).IsRequired();
            entity.Property(e => e.PhoneNumber).HasMaxLength(20);
            entity.Property(e => e.AuthMethod).HasMaxLength(50);
            entity.Property(e => e.LeadershipLevel).HasMaxLength(100);
            entity.Property(e => e.Tenant).HasMaxLength(100);
            entity.HasIndex(e => e.NormalizedUserName).IsUnique();
            entity.HasIndex(e => e.NormalizedEmail).IsUnique();
            entity.HasQueryFilter(e => !e.IsDeleted);

            entity.HasOne(c => c.Clients)
                .WithMany(co => co.AspNetUsers)
                .HasForeignKey(c => c.ClientsId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<WellbeingDimension>(entity =>
        {
            entity.ToTable("WellbeingDimensions");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.ClientsId).IsRequired();
            entity.HasIndex(e => e.ClientsId);
            entity.HasQueryFilter(e => !e.IsDeleted);

            entity.HasOne(wd => wd.Clients)
                .WithMany(c => c.WellbeingDimensions)
                .HasForeignKey(wd => wd.ClientsId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<WellbeingSubDimension>(entity =>
        {
            entity.ToTable("WellbeingSubDimensions");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.WellbeingDimensionId).IsRequired();
            entity.Property(e => e.ClientsId).IsRequired();
            entity.HasIndex(e => e.WellbeingDimensionId);
            entity.HasIndex(e => e.ClientsId);
            entity.HasQueryFilter(e => !e.IsDeleted);

            entity.HasOne(wsd => wsd.WellbeingDimension)
                .WithMany(wd => wd.WellbeingSubDimensions)
                .HasForeignKey(wsd => wsd.WellbeingDimensionId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(wsd => wsd.Clients)
                .WithMany()
                .HasForeignKey(wsd => wsd.ClientsId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Question>(entity =>
        {
            entity.ToTable("Questions");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.QuestionText).IsRequired().HasMaxLength(1000);
            entity.Property(e => e.QuestionType).HasMaxLength(50);
            entity.Property(e => e.WellbeingDimensionId).IsRequired();
            entity.Property(e => e.WellbeingSubDimensionId).IsRequired();
            entity.Property(e => e.ClientsId).IsRequired();
            entity.HasIndex(e => e.WellbeingDimensionId);
            entity.HasIndex(e => e.WellbeingSubDimensionId);
            entity.HasIndex(e => e.ClientsId);
            entity.HasQueryFilter(e => !e.IsDeleted);

            entity.HasOne(q => q.WellbeingDimension)
                .WithMany(wd => wd.Questions)
                .HasForeignKey(q => q.WellbeingDimensionId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(q => q.WellbeingSubDimension)
                .WithMany(wsd => wsd.Questions)
                .HasForeignKey(q => q.WellbeingSubDimensionId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(q => q.Clients)
                .WithMany()
                .HasForeignKey(q => q.ClientsId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    break;
                case EntityState.Modified:
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                    break;
            }
        }

        foreach (var entry in ChangeTracker.Entries<AspNetUsers>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    if (entry.Entity.Id == Guid.Empty)
                    {
                        entry.Entity.Id = Guid.NewGuid();
                    }
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    break;
                case EntityState.Modified:
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                    break;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}
