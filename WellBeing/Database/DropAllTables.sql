-- =============================================
-- Wellbeing Database - Drop All Tables Script
-- =============================================
-- WARNING: This script will DROP ALL TABLES and their data!
-- Use with caution. This is useful for resetting the database.
-- =============================================

-- Connect to the database (uncomment if running from psql)
-- \c "WellbeingDb"

-- =============================================
-- Drop tables in reverse dependency order
-- =============================================

-- Drop QuestionResponses first (depends on Questions, AspNetUsers, Clients)
DROP TABLE IF EXISTS "QuestionResponses" CASCADE;

-- Drop Questions (depends on Surveys, Clients, WellbeingDimensions, WellbeingSubDimensions)
DROP TABLE IF EXISTS "Questions" CASCADE;

-- Drop AspNetUsers (depends on Clients)
DROP TABLE IF EXISTS "AspNetUsers" CASCADE;

-- Drop Surveys (depends on Clients)
DROP TABLE IF EXISTS "Surveys" CASCADE;

-- Drop WellbeingSubDimensions (depends on WellbeingDimensions, Clients)
DROP TABLE IF EXISTS "WellbeingSubDimensions" CASCADE;

-- Drop WellbeingDimensions (depends on Clients)
DROP TABLE IF EXISTS "WellbeingDimensions" CASCADE;

-- Drop Clients last (no dependencies)
DROP TABLE IF EXISTS "Clients" CASCADE;

-- =============================================
-- Verification
-- =============================================
DO $$
BEGIN
    RAISE NOTICE '=============================================';
    RAISE NOTICE 'All tables dropped successfully!';
    RAISE NOTICE '=============================================';
END $$;

-- Verify tables are dropped
SELECT 
    table_name AS "Remaining Tables"
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
    );

-- If the query returns no rows, all tables were successfully dropped

-- =============================================
-- End of Script
-- =============================================
