-- =============================================
-- Fix Migration History Script
-- =============================================
-- This script helps fix the __EFMigrationsHistory table
-- when tables exist but migrations aren't recorded
-- =============================================

-- Connect to the database (uncomment if running from psql)
-- \c "WellbeingDb"

-- =============================================
-- 1. Check current migration history
-- =============================================
SELECT 
    "MigrationId",
    "ProductVersion"
FROM "__EFMigrationsHistory"
ORDER BY "MigrationId";

-- =============================================
-- 2. Check which tables exist
-- =============================================
SELECT 
    table_name AS "Existing Tables"
FROM information_schema.tables 
WHERE table_schema = 'public' 
    AND table_type = 'BASE TABLE'
    AND table_name IN (
        'Clients', 
        'AspNetUsers', 
        'Surveys', 
        'WellbeingDimensions', 
        'WellbeingSubDimensions', 
        'Questions', 
        'QuestionResponses'
    )
ORDER BY table_name;

-- =============================================
-- 3. Create __EFMigrationsHistory table if it doesn't exist
-- =============================================
CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" VARCHAR(150) NOT NULL PRIMARY KEY,
    "ProductVersion" VARCHAR(32) NOT NULL
);

-- =============================================
-- 4. Insert the migration record if it doesn't exist
-- =============================================
-- Replace '20250130000000_InitialCreate' with your actual migration name
INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20250130000000_InitialCreate', '8.0.11')
ON CONFLICT ("MigrationId") DO NOTHING;

-- =============================================
-- 5. Verify the migration is recorded
-- =============================================
SELECT 
    'Migration History' AS "Check Type",
    CASE 
        WHEN COUNT(*) > 0 THEN 'PASS - Migration recorded'
        ELSE 'FAIL - No migrations found'
    END AS "Status",
    COUNT(*) AS "Migration Count"
FROM "__EFMigrationsHistory"
WHERE "MigrationId" = '20250130000000_InitialCreate';

-- =============================================
-- Alternative: If you want to reset everything
-- =============================================
-- Uncomment the following to drop all tables and start fresh:
-- 
-- DROP TABLE IF EXISTS "QuestionResponses" CASCADE;
-- DROP TABLE IF EXISTS "Questions" CASCADE;
-- DROP TABLE IF EXISTS "AspNetUsers" CASCADE;
-- DROP TABLE IF EXISTS "Surveys" CASCADE;
-- DROP TABLE IF EXISTS "WellbeingSubDimensions" CASCADE;
-- DROP TABLE IF EXISTS "WellbeingDimensions" CASCADE;
-- DROP TABLE IF EXISTS "Clients" CASCADE;
-- DELETE FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250130000000_InitialCreate';
--
-- Then run the migration again or use CreateAllTables.sql

-- =============================================
-- End of Script
-- =============================================
