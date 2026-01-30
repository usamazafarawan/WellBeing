# WellBeing API - Windows Setup Guide

This guide will walk you through setting up the WellBeing API project on a Windows machine from scratch.

## Table of Contents

1. [Prerequisites](#prerequisites)
2. [Installation Steps](#installation-steps)
3. [Database Setup](#database-setup)
4. [Configuration](#configuration)
5. [Running the Application](#running-the-application)
6. [API Documentation](#api-documentation)
7. [Project Structure](#project-structure)
8. [Troubleshooting](#troubleshooting)

---

## Prerequisites

Before you begin, ensure you have the following software installed on your Windows machine:

### 1. .NET 8.0 SDK

The project requires .NET SDK version 8.0.404 or later (as specified in `global.json`).

**Download and Install:**
1. Visit [https://dotnet.microsoft.com/download/dotnet/8.0](https://dotnet.microsoft.com/download/dotnet/8.0)
2. Download the **.NET 8.0 SDK** (not just the runtime)
3. Run the installer and follow the installation wizard
4. Restart your computer if prompted

**Verify Installation:**
Open PowerShell or Command Prompt and run:
```powershell
dotnet --version
```
You should see version `8.0.404` or higher.

### 2. PostgreSQL Database

The application uses PostgreSQL as its database. You need to install PostgreSQL server.

**Download and Install:**
1. Visit [https://www.postgresql.org/download/windows/](https://www.postgresql.org/download/windows/)
2. Download the PostgreSQL installer (recommended: PostgreSQL 14 or later)
3. Run the installer:
   - Choose an installation directory (default is fine)
   - **Important:** During installation, you'll be asked to set a password for the `postgres` superuser
   - **Remember this password** - you'll need it for the connection string
   - Default port is `5432` (keep this unless you have a conflict)
   - Complete the installation

**Verify Installation:**
Open PowerShell and run:
```powershell
psql --version
```

**Note:** If `psql` is not recognized, you may need to add PostgreSQL's `bin` directory to your PATH environment variable:
- Default location: `C:\Program Files\PostgreSQL\<version>\bin`
- Add it to PATH: System Properties → Environment Variables → Edit PATH → Add the bin directory

### 3. Git (Optional but Recommended)

If you're cloning the repository, you'll need Git.

**Download and Install:**
1. Visit [https://git-scm.com/download/win](https://git-scm.com/download/win)
2. Download and run the installer
3. Use default settings during installation

### 4. IDE (Recommended)

While not required, using an IDE will make development easier:

- **Visual Studio 2022** (Community edition is free)
  - Download from: [https://visualstudio.microsoft.com/downloads/](https://visualstudio.microsoft.com/downloads/)
  - During installation, select the **"ASP.NET and web development"** workload
- **Visual Studio Code** (Free, lightweight)
  - Download from: [https://code.visualstudio.com/](https://code.visualstudio.com/)
  - Install the **C#** extension

---

## Installation Steps

### Step 1: Clone or Download the Repository

If you have Git installed:
```powershell
git clone <repository-url>
cd WellBeing
```

If you don't have Git, download the project as a ZIP file and extract it to your desired location.

### Step 2: Verify Project Structure

Ensure you have the following folder structure:
```
WellBeing/
├── Wellbeing.API/
├── Wellbeing.Application/
├── Wellbeing.Domain/
├── Wellbeing.Infrastructure/
├── global.json
└── Wellbeing.sln
```

### Step 3: Restore NuGet Packages

Open PowerShell or Command Prompt in the project root directory (`WellBeing`) and run:

```powershell
dotnet restore
```

This will download all required NuGet packages for all projects in the solution.

**Expected Output:**
You should see messages indicating that packages are being restored for each project. This may take a few minutes on the first run.

---

## Database Setup

### Step 1: Create the Database

1. Open **pgAdmin** (installed with PostgreSQL) or use the command line.

**Using pgAdmin:**
- Launch pgAdmin from the Start menu
- Connect to your PostgreSQL server (use the password you set during installation)
- Right-click on "Databases" → "Create" → "Database"
- Name: `WellbeingDb`
- Click "Save"

**Using Command Line (psql):**
```powershell
psql -U postgres
```
Enter your postgres password when prompted, then:
```sql
CREATE DATABASE "WellbeingDb";
\q
```

### Step 2: Update Connection String

1. Navigate to `Wellbeing.API/appsettings.json`
2. Update the connection string with your PostgreSQL credentials:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=WellbeingDb;Username=postgres;Password=YOUR_POSTGRES_PASSWORD"
  }
}
```

**Replace `YOUR_POSTGRES_PASSWORD`** with the password you set during PostgreSQL installation.

**Also update** `Wellbeing.API/appsettings.Development.json` with the same connection string.

### Step 3: Run Database Migrations

Entity Framework Core migrations are already created in the `Wellbeing.Infrastructure/Migrations/` folder. You have two options to apply them:

#### Option A: Automatic Migration (Recommended for Development)

The application is configured to automatically apply pending migrations when running in Development mode. Simply start the application:

```powershell
cd Wellbeing.API
dotnet run
```

The migrations will be applied automatically on startup.

#### Option B: Manual Migration

If you prefer to run migrations manually:

1. Navigate to the `Wellbeing.API` project directory:
```powershell
cd Wellbeing.API
```

2. Run the migrations:
```powershell
dotnet ef database update
```

**Note:** If you get an error saying `dotnet ef` is not found, install the EF Core tools globally:
```powershell
dotnet tool install --global dotnet-ef
```

Then try the migration command again.

**Expected Output:**
You should see messages indicating that migrations are being applied. The database tables will be created automatically.

**Verify Database:**
You can verify the tables were created by checking in pgAdmin:
- Expand `WellbeingDb` → `Schemas` → `public` → `Tables`
- You should see tables: `Clients`, `AspNetUsers`, `WellbeingDimensions`, `WellbeingSubDimensions`, `Questions`

---

## Configuration

### Environment-Specific Settings

The project uses different configuration files for different environments:

- **`appsettings.json`** - Base configuration (used in all environments)
- **`appsettings.Development.json`** - Development-specific overrides

### Connection String Format

The connection string format for PostgreSQL is:
```
Host=localhost;Port=5432;Database=WellbeingDb;Username=postgres;Password=your_password
```

**Security Note:** For production, never commit passwords to source control. Use:
- User Secrets (for development)
- Environment Variables
- Azure Key Vault or similar secure storage

### CORS Configuration

The application is configured to allow all origins, methods, and headers in development. This is set in `Program.cs`:
```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});
```

For production, you should restrict CORS to specific origins.

---

## Running the Application

### Option 1: Using Command Line

1. Navigate to the API project:
```powershell
cd Wellbeing.API
```

2. Run the application:
```powershell
dotnet run
```

The application will start and display the URLs it's listening on, typically:
- HTTP: `http://localhost:5000`
- HTTPS: `https://localhost:5001`

### Option 2: Using Visual Studio

1. Open `Wellbeing.sln` in Visual Studio
2. Set `Wellbeing.API` as the startup project (right-click → "Set as Startup Project")
3. Press `F5` or click the "Run" button

### Option 3: Using Visual Studio Code

1. Open the `WellBeing` folder in VS Code
2. Open the terminal (`` Ctrl+` ``)
3. Navigate to `Wellbeing.API`:
```powershell
cd Wellbeing.API
```
4. Run:
```powershell
dotnet run
```

### Verify the Application is Running

1. Open your web browser
2. Navigate to: `https://localhost:5001/swagger` (or `http://localhost:5000/swagger` if HTTPS is not available)
3. You should see the Swagger UI with all available API endpoints

**Note:** If you see a certificate warning for HTTPS, click "Advanced" → "Proceed to localhost" (this is normal for development).

---

## API Documentation

### Swagger UI

When running in Development mode, Swagger UI is automatically available at:
- `https://localhost:5001/swagger`
- `http://localhost:5000/swagger`

Swagger provides:
- Interactive API documentation
- Ability to test endpoints directly
- Request/response schemas

### Available Controllers

The API includes the following controllers:
- **AspNetUsersController** - User management
- **ClientsController** - Client management
- **QuestionsController** - Question management
- **WellbeingDimensionsController** - Wellbeing dimensions management
- **WellbeingSubDimensionsController** - Wellbeing sub-dimensions management


### Key Technologies

- **.NET 8.0** - Framework
- **Entity Framework Core 8.0.11** - ORM
- **PostgreSQL** - Database (via Npgsql)
- **MediatR** - CQRS pattern implementation
- **AutoMapper** - Object mapping
- **FluentValidation** - Input validation
- **Swagger** - API documentation

---

## Troubleshooting

### Issue: `dotnet` command not found

**Solution:**
- Ensure .NET SDK is installed
- Restart your terminal/PowerShell after installation
- Verify installation: `dotnet --version`
- If still not found, manually add .NET to PATH:
  - Default location: `C:\Program Files\dotnet`
  - Add to System PATH environment variable

### Issue: `dotnet ef` command not found

**Solution:**
Install EF Core tools globally:
```powershell
dotnet tool install --global dotnet-ef
```

### Issue: Cannot connect to PostgreSQL

**Common Causes and Solutions:**

1. **PostgreSQL service not running:**
   - Open Services (Win+R → `services.msc`)
   - Find "postgresql-x64-XX" service
   - Right-click → Start

2. **Wrong password:**
   - Verify the password in `appsettings.json` matches your PostgreSQL password
   - Reset password if needed (using pgAdmin or psql)

3. **Wrong port:**
   - Default is 5432
   - Check if another service is using this port
   - Update connection string if using a different port

4. **Database doesn't exist:**
   - Create the database as described in [Database Setup](#database-setup)

5. **Firewall blocking connection:**
   - Ensure Windows Firewall allows PostgreSQL (port 5432)
   - Or temporarily disable firewall for testing

### Issue: Migration errors or "relation does not exist" error

**Error Message:** `42P01: relation "Clients" does not exist` or similar table not found errors.

**This error occurs when:**
- Migrations haven't been applied to the database
- The database is in an inconsistent state
- The application tries to access tables before migrations complete

**Solutions (try in order):**

1. **Automatic Migration (Easiest):**
   - The application automatically applies migrations in Development mode
   - Simply run the application: `dotnet run` from the `Wellbeing.API` directory
   - The migrations will be applied on startup

2. **Manual Migration:**
   - Navigate to `Wellbeing.API` directory
   - Run: `dotnet ef database update`
   - If `dotnet ef` is not found, install it: `dotnet tool install --global dotnet-ef`

3. **Reset Database (if migrations are corrupted):**
   - **Warning:** This will delete all data!
   - Connect to PostgreSQL using pgAdmin or psql
   - Drop and recreate the database:
     ```sql
     DROP DATABASE IF EXISTS "WellbeingDb";
     CREATE DATABASE "WellbeingDb";
     ```
   - Then run the application or `dotnet ef database update`

4. **Check Migration Status:**
   - Verify migrations exist in `Wellbeing.Infrastructure/Migrations/`
   - Check if `__EFMigrationsHistory` table exists in your database
   - If it exists, check which migrations have been applied:
     ```sql
     SELECT * FROM "__EFMigrationsHistory";
     ```

5. **Verify Connection String:**
   - Ensure `appsettings.json` and `appsettings.Development.json` have correct connection strings
   - Test connection using pgAdmin or psql

6. **Clean and Rebuild:**
   ```powershell
   dotnet clean
   dotnet restore
   dotnet build
   dotnet ef database update --project ../Wellbeing.Infrastructure
   ```

### Issue: Port already in use

**Solution:**
1. Find what's using the port:
   ```powershell
   netstat -ano | findstr :5000
   ```
2. Either:
   - Stop the conflicting application
   - Or change the port in `Properties/launchSettings.json`

### Issue: SSL/Certificate errors

**Solution:**
- For development, this is normal
- Click "Advanced" → "Proceed" in your browser
- Or use HTTP instead of HTTPS (update `launchSettings.json`)

### Issue: NuGet package restore fails

**Solution:**
1. Clear NuGet cache:
   ```powershell
   dotnet nuget locals all --clear
   ```
2. Restore again:
   ```powershell
   dotnet restore
   ```
3. Check your internet connection
4. Verify you're not behind a corporate firewall blocking NuGet

### Issue: Build errors

**Solution:**
1. Clean the solution:
   ```powershell
   dotnet clean
   ```
2. Restore packages:
   ```powershell
   dotnet restore
   ```
3. Rebuild:
   ```powershell
   dotnet build
   ```
4. Check that all projects target the same .NET version (should be 8.0)

---

## Additional Resources

### Learning Resources

- [.NET Documentation](https://learn.microsoft.com/dotnet/)
- [Entity Framework Core Documentation](https://learn.microsoft.com/ef/core/)
- [PostgreSQL Documentation](https://www.postgresql.org/docs/)
- [Clean Architecture Guide](https://learn.microsoft.com/dotnet/architecture/modern-web-apps-azure/common-web-application-architectures)

### Useful Commands

**Create a new migration:**
```powershell
cd Wellbeing.API
dotnet ef migrations add MigrationName --project ../Wellbeing.Infrastructure
```

**Remove the last migration:**
```powershell
dotnet ef migrations remove --project ../Wellbeing.Infrastructure
```

**Update database:**
```powershell
dotnet ef database update --project ../Wellbeing.Infrastructure
```

**Build the solution:**
```powershell
dotnet build
```

**Run tests (if any):**
```powershell
dotnet test
```

---

## Support

If you encounter issues not covered in this guide:

1. Check the error messages carefully - they often contain helpful information
2. Verify all prerequisites are installed correctly
3. Ensure all configuration files are set up properly
4. Check that the database is running and accessible
5. Review the project's issue tracker (if available)

---

## Next Steps

After successfully setting up the project:

1. Explore the API using Swagger UI
2. Review the code structure to understand the architecture
3. Check the existing migrations to understand the database schema
4. Start developing new features following the existing patterns

---

**Happy Coding! 🚀**
