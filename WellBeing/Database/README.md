# Database Scripts

This directory contains SQL scripts for manually creating and managing the Wellbeing database schema.

## Scripts Overview

### 1. `CreateAllTables.sql`
**Purpose**: Creates all database tables from scratch.

**What it does**:
- Creates all 7 tables in the correct order (respecting foreign key dependencies)
- Creates all indexes
- Creates all foreign key constraints
- Adds table and column comments

**Tables created**:
1. `Clients` - Client/company information
2. `AspNetUsers` - User accounts and authentication
3. `Surveys` - Survey definitions
4. `WellbeingDimensions` - Wellbeing dimension categories
5. `WellbeingSubDimensions` - Sub-dimensions within dimensions
6. `Questions` - Survey questions
7. `QuestionResponses` - User responses to questions

**Usage**:
```bash
# Using psql
psql -U postgres -d WellbeingDb -f CreateAllTables.sql

# Or connect first, then run
psql -U postgres
\c WellbeingDb
\i CreateAllTables.sql
```

### 2. `DropAllTables.sql`
**Purpose**: Drops all tables (useful for resetting the database).

**⚠️ WARNING**: This will delete all data!

**Usage**:
```bash
psql -U postgres -d WellbeingDb -f DropAllTables.sql
```

### 3. `VerifySchema.sql`
**Purpose**: Verifies that the database schema is correctly created.

**What it checks**:
- Table existence
- Column counts
- Foreign key constraints
- Indexes
- Primary keys
- Summary report

**Usage**:
```bash
psql -U postgres -d WellbeingDb -f VerifySchema.sql
```

## Quick Start

### Option 1: Fresh Database Setup
```bash
# 1. Create the database (if it doesn't exist)
psql -U postgres -c "CREATE DATABASE \"WellbeingDb\";"

# 2. Create all tables
psql -U postgres -d WellbeingDb -f CreateAllTables.sql

# 3. Verify the schema
psql -U postgres -d WellbeingDb -f VerifySchema.sql
```

### Option 2: Reset Existing Database
```bash
# 1. Drop all tables
psql -U postgres -d WellbeingDb -f DropAllTables.sql

# 2. Recreate all tables
psql -U postgres -d WellbeingDb -f CreateAllTables.sql

# 3. Verify the schema
psql -U postgres -d WellbeingDb -f VerifySchema.sql
```

## Table Dependencies

The tables must be created in this order due to foreign key dependencies:

```
Clients (no dependencies)
    ├── AspNetUsers
    ├── Surveys
    ├── WellbeingDimensions
    │       └── WellbeingSubDimensions
    │               └── Questions
    └── Questions (also depends on Surveys)
            └── QuestionResponses (also depends on AspNetUsers)
```

## Notes

- All scripts use `IF NOT EXISTS` and `IF EXISTS` clauses to be idempotent
- Foreign keys use `ON DELETE RESTRICT` to prevent accidental data deletion
- Indexes are created for performance optimization
- JSONB columns are used for flexible configuration storage
- UUID is used for AspNetUsers.Id with automatic generation
- All tables include `IsDeleted` for soft delete functionality
- Timestamps use `TIMESTAMP WITH TIME ZONE` for proper timezone handling

## Troubleshooting

### Error: "relation already exists"
- The table already exists. Use `DropAllTables.sql` first, or modify the script to use `DROP TABLE IF EXISTS` before creating.

### Error: "foreign key constraint violation"
- Make sure tables are created in the correct order (as shown in dependencies above).

### Error: "extension uuid-ossp does not exist"
- Run: `CREATE EXTENSION IF NOT EXISTS "uuid-ossp";` manually, or ensure PostgreSQL version supports `gen_random_uuid()`.

### Error: "permission denied"
- Make sure you're connected as a user with sufficient privileges (usually `postgres` superuser).

## Alternative: Using Entity Framework Migrations

Instead of running these scripts manually, you can use Entity Framework Core migrations:

```bash
cd Wellbeing.API
dotnet ef database update
```

The migrations will automatically create all tables. These SQL scripts are provided as an alternative for manual database management.
