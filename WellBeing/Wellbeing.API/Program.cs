using Wellbeing.Application;
using Wellbeing.Infrastructure;
using Wellbeing.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplication();

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Apply pending migrations automatically in Development
if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        var logger = services.GetRequiredService<ILogger<Program>>();
        try
        {
            var context = services.GetRequiredService<ApplicationDbContext>();
            
            // Check if database can connect
            logger.LogInformation("Checking database connection...");
            if (context.Database.CanConnect())
            {
                logger.LogInformation("Database connection successful.");
            }
            else
            {
                logger.LogWarning("Cannot connect to database. Migrate() will attempt to create it if it doesn't exist.");
            }
            
            // Get migration status
            var appliedMigrations = context.Database.GetAppliedMigrations().ToList();
            var pendingMigrations = context.Database.GetPendingMigrations().ToList();
            
            if (appliedMigrations.Any())
            {
                logger.LogInformation("Applied migrations ({Count}): {Migrations}", 
                    appliedMigrations.Count, 
                    string.Join(", ", appliedMigrations));
            }
            
            if (pendingMigrations.Any())
            {
                logger.LogInformation("Pending migrations ({Count}): {Migrations}", 
                    pendingMigrations.Count, 
                    string.Join(", ", pendingMigrations));
            }
            else
            {
                logger.LogInformation("No pending migrations. Database is up to date.");
            }
            
            // Apply migrations - this will create the database if it doesn't exist
            logger.LogInformation("Applying migrations...");
            context.Database.Migrate();
            logger.LogInformation("Migrations applied successfully.");
        }
        catch (Npgsql.PostgresException ex) when (ex.SqlState == "42P01")
        {
            logger.LogError(ex, 
                "Migration error: Table does not exist (42P01). This usually means migrations are in an inconsistent state. " +
                "Solution: Drop and recreate the database, then restart the application. " +
                "SQL: DROP DATABASE IF EXISTS \"WellbeingDb\"; CREATE DATABASE \"WellbeingDb\";");
            throw; // Re-throw to prevent app from starting with bad database state
        }
        catch (Exception ex) when (ex.Message.Contains("does not exist") || ex.Message.Contains("relation") || ex.Message.Contains("42P01"))
        {
            logger.LogError(ex, 
                "Migration error: Table does not exist (42P01). This usually means migrations are in an inconsistent state. " +
                "Solution: Drop and recreate the database, then restart the application. " +
                "SQL: DROP DATABASE IF EXISTS \"WellbeingDb\"; CREATE DATABASE \"WellbeingDb\";");
            throw; // Re-throw to prevent app from starting with bad database state
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while migrating the database. " +
                "If you see 'relation does not exist' errors, you may need to reset the database. " +
                "See README.md troubleshooting section for details.");
            throw; // Re-throw to prevent app from starting with bad database state
        }
    }

    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseMiddleware<Wellbeing.API.Middleware.ExceptionHandlingMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();
