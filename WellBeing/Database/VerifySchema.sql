-- =============================================
-- Wellbeing Database - Schema Verification Script
-- =============================================
-- This script verifies that all tables, columns, indexes, and constraints
-- are correctly created in the database.
-- =============================================

-- Connect to the database (uncomment if running from psql)
-- \c "WellbeingDb"

-- =============================================
-- 1. Check if all tables exist
-- =============================================
SELECT 
    'Table Existence Check' AS "Check Type",
    CASE 
        WHEN COUNT(*) = 7 THEN 'PASS - All 7 tables exist'
        ELSE 'FAIL - Missing tables. Expected 7, found ' || COUNT(*)::TEXT
    END AS "Status"
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

-- =============================================
-- 2. List all tables with column counts
-- =============================================
SELECT 
    t.table_name AS "Table Name",
    COUNT(c.column_name) AS "Column Count",
    CASE 
        WHEN t.table_name = 'Clients' AND COUNT(c.column_name) = 8 THEN '✓'
        WHEN t.table_name = 'AspNetUsers' AND COUNT(c.column_name) = 24 THEN '✓'
        WHEN t.table_name = 'Surveys' AND COUNT(c.column_name) = 9 THEN '✓'
        WHEN t.table_name = 'WellbeingDimensions' AND COUNT(c.column_name) = 7 THEN '✓'
        WHEN t.table_name = 'WellbeingSubDimensions' AND COUNT(c.column_name) = 8 THEN '✓'
        WHEN t.table_name = 'Questions' AND COUNT(c.column_name) = 12 THEN '✓'
        WHEN t.table_name = 'QuestionResponses' AND COUNT(c.column_name) = 9 THEN '✓'
        ELSE '✗'
    END AS "Status"
FROM information_schema.tables t
LEFT JOIN information_schema.columns c 
    ON t.table_name = c.table_name 
    AND t.table_schema = c.table_schema
WHERE t.table_schema = 'public' 
    AND t.table_type = 'BASE TABLE'
    AND t.table_name IN (
        'Clients', 
        'AspNetUsers', 
        'Surveys', 
        'WellbeingDimensions', 
        'WellbeingSubDimensions', 
        'Questions', 
        'QuestionResponses'
    )
GROUP BY t.table_name
ORDER BY 
    CASE t.table_name
        WHEN 'Clients' THEN 1
        WHEN 'AspNetUsers' THEN 2
        WHEN 'Surveys' THEN 3
        WHEN 'WellbeingDimensions' THEN 4
        WHEN 'WellbeingSubDimensions' THEN 5
        WHEN 'Questions' THEN 6
        WHEN 'QuestionResponses' THEN 7
    END;

-- =============================================
-- 3. Check Foreign Key Constraints
-- =============================================
SELECT 
    tc.table_name AS "Table",
    kcu.column_name AS "Column",
    ccu.table_name AS "Referenced Table",
    ccu.column_name AS "Referenced Column",
    tc.constraint_name AS "Constraint Name"
FROM information_schema.table_constraints AS tc
JOIN information_schema.key_column_usage AS kcu
    ON tc.constraint_name = kcu.constraint_name
    AND tc.table_schema = kcu.table_schema
JOIN information_schema.constraint_column_usage AS ccu
    ON ccu.constraint_name = tc.constraint_name
    AND ccu.table_schema = tc.table_schema
WHERE tc.constraint_type = 'FOREIGN KEY'
    AND tc.table_schema = 'public'
    AND tc.table_name IN (
        'Clients', 
        'AspNetUsers', 
        'Surveys', 
        'WellbeingDimensions', 
        'WellbeingSubDimensions', 
        'Questions', 
        'QuestionResponses'
    )
ORDER BY tc.table_name, kcu.column_name;

-- =============================================
-- 4. Check Indexes
-- =============================================
SELECT 
    schemaname AS "Schema",
    tablename AS "Table",
    indexname AS "Index Name",
    indexdef AS "Index Definition"
FROM pg_indexes
WHERE schemaname = 'public'
    AND tablename IN (
        'Clients', 
        'AspNetUsers', 
        'Surveys', 
        'WellbeingDimensions', 
        'WellbeingSubDimensions', 
        'Questions', 
        'QuestionResponses'
    )
ORDER BY tablename, indexname;

-- =============================================
-- 5. Check Primary Keys
-- =============================================
SELECT 
    tc.table_name AS "Table",
    kcu.column_name AS "Primary Key Column",
    tc.constraint_name AS "Constraint Name"
FROM information_schema.table_constraints AS tc
JOIN information_schema.key_column_usage AS kcu
    ON tc.constraint_name = kcu.constraint_name
    AND tc.table_schema = kcu.table_schema
WHERE tc.constraint_type = 'PRIMARY KEY'
    AND tc.table_schema = 'public'
    AND tc.table_name IN (
        'Clients', 
        'AspNetUsers', 
        'Surveys', 
        'WellbeingDimensions', 
        'WellbeingSubDimensions', 
        'Questions', 
        'QuestionResponses'
    )
ORDER BY tc.table_name;

-- =============================================
-- 6. Summary Report
-- =============================================
SELECT 
    'Summary' AS "Report Type",
    (SELECT COUNT(*) FROM information_schema.tables 
     WHERE table_schema = 'public' 
     AND table_name IN ('Clients', 'AspNetUsers', 'Surveys', 'WellbeingDimensions', 
                        'WellbeingSubDimensions', 'Questions', 'QuestionResponses')) AS "Tables",
    (SELECT COUNT(*) FROM information_schema.table_constraints 
     WHERE constraint_type = 'PRIMARY KEY' 
     AND table_schema = 'public'
     AND table_name IN ('Clients', 'AspNetUsers', 'Surveys', 'WellbeingDimensions', 
                        'WellbeingSubDimensions', 'Questions', 'QuestionResponses')) AS "Primary Keys",
    (SELECT COUNT(*) FROM information_schema.table_constraints 
     WHERE constraint_type = 'FOREIGN KEY' 
     AND table_schema = 'public'
     AND table_name IN ('Clients', 'AspNetUsers', 'Surveys', 'WellbeingDimensions', 
                        'WellbeingSubDimensions', 'Questions', 'QuestionResponses')) AS "Foreign Keys",
    (SELECT COUNT(*) FROM pg_indexes 
     WHERE schemaname = 'public'
     AND tablename IN ('Clients', 'AspNetUsers', 'Surveys', 'WellbeingDimensions', 
                       'WellbeingSubDimensions', 'Questions', 'QuestionResponses')) AS "Indexes";

-- =============================================
-- End of Script
-- =============================================
