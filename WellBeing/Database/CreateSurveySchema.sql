-- =============================================
-- Create Survey and Update Questions Schema
-- =============================================
-- This script creates the Survey table and updates Questions to link to Surveys
-- Run this after the base database is created
-- =============================================

-- Connect to the database
\c "WellbeingDb"

-- =============================================
-- 1. Create Surveys Table
-- =============================================
CREATE TABLE IF NOT EXISTS "Surveys" (
    "Id" SERIAL PRIMARY KEY,
    "Title" VARCHAR(500) NOT NULL,
    "Description" VARCHAR(2000) NULL,
    "ClientsId" INTEGER NOT NULL,
    "IsActive" BOOLEAN NOT NULL DEFAULT TRUE,
    "StartDate" TIMESTAMP WITH TIME ZONE NULL,
    "EndDate" TIMESTAMP WITH TIME ZONE NULL,
    "CreatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT (NOW() AT TIME ZONE 'utc'),
    "UpdatedAt" TIMESTAMP WITH TIME ZONE NULL,
    "IsDeleted" BOOLEAN NOT NULL DEFAULT FALSE,
    CONSTRAINT "FK_Surveys_Clients_ClientsId" 
        FOREIGN KEY ("ClientsId") 
        REFERENCES "Clients" ("Id") 
        ON DELETE RESTRICT
);

-- Indexes for Surveys table
CREATE INDEX IF NOT EXISTS "IX_Surveys_ClientsId" ON "Surveys" ("ClientsId");
CREATE INDEX IF NOT EXISTS "IX_Surveys_IsActive" ON "Surveys" ("IsActive", "IsDeleted") WHERE "IsDeleted" = FALSE;
CREATE INDEX IF NOT EXISTS "IX_Surveys_IsDeleted" ON "Surveys" ("IsDeleted") WHERE "IsDeleted" = FALSE;

COMMENT ON TABLE "Surveys" IS 'Stores survey information linked to clients';
COMMENT ON COLUMN "Surveys"."IsActive" IS 'Whether the survey is currently active and accepting responses';

-- =============================================
-- 2. Update Questions Table
-- =============================================
-- Add SurveyId column (make it required)
ALTER TABLE "Questions" 
    ADD COLUMN IF NOT EXISTS "SurveyId" INTEGER NULL,
    ADD COLUMN IF NOT EXISTS "QuestionConfig" JSONB NULL,
    ADD COLUMN IF NOT EXISTS "IsRequired" BOOLEAN NOT NULL DEFAULT TRUE,
    ADD COLUMN IF NOT EXISTS "DisplayOrder" INTEGER NOT NULL DEFAULT 0;

-- Update QuestionText length to support longer questions
ALTER TABLE "Questions" 
    ALTER COLUMN "QuestionText" TYPE VARCHAR(2000);

-- Make WellbeingDimensionId and WellbeingSubDimensionId nullable (optional now)
ALTER TABLE "Questions" 
    ALTER COLUMN "WellbeingDimensionId" DROP NOT NULL,
    ALTER COLUMN "WellbeingSubDimensionId" DROP NOT NULL;

-- Add foreign key for SurveyId
DO $$
BEGIN
    IF NOT EXISTS (
        SELECT 1 FROM information_schema.table_constraints 
        WHERE constraint_name = 'FK_Questions_Surveys_SurveyId'
    ) THEN
        ALTER TABLE "Questions" 
            ADD CONSTRAINT "FK_Questions_Surveys_SurveyId" 
            FOREIGN KEY ("SurveyId") 
            REFERENCES "Surveys" ("Id") 
            ON DELETE RESTRICT;
    END IF;
END $$;

-- Create index for SurveyId
CREATE INDEX IF NOT EXISTS "IX_Questions_SurveyId" ON "Questions" ("SurveyId");
CREATE INDEX IF NOT EXISTS "IX_Questions_DisplayOrder" ON "Questions" ("SurveyId", "DisplayOrder");

-- =============================================
-- 3. Create QuestionResponses Table
-- =============================================
CREATE TABLE IF NOT EXISTS "QuestionResponses" (
    "Id" SERIAL PRIMARY KEY,
    "QuestionId" INTEGER NOT NULL,
    "AspNetUsersId" UUID NOT NULL,
    "ClientsId" INTEGER NOT NULL,
    "ComponentType" VARCHAR(50) NOT NULL,
    "ComponentIndex" INTEGER NOT NULL DEFAULT 0,
    "ResponseValue" JSONB NOT NULL,
    "CreatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT (NOW() AT TIME ZONE 'utc'),
    "UpdatedAt" TIMESTAMP WITH TIME ZONE NULL,
    "IsDeleted" BOOLEAN NOT NULL DEFAULT FALSE,
    CONSTRAINT "FK_QuestionResponses_Questions_QuestionId" 
        FOREIGN KEY ("QuestionId") 
        REFERENCES "Questions" ("Id") 
        ON DELETE RESTRICT,
    CONSTRAINT "FK_QuestionResponses_AspNetUsers_AspNetUsersId" 
        FOREIGN KEY ("AspNetUsersId") 
        REFERENCES "AspNetUsers" ("Id") 
        ON DELETE RESTRICT,
    CONSTRAINT "FK_QuestionResponses_Clients_ClientsId" 
        FOREIGN KEY ("ClientsId") 
        REFERENCES "Clients" ("Id") 
        ON DELETE RESTRICT
);

-- Indexes for QuestionResponses
CREATE INDEX IF NOT EXISTS "IX_QuestionResponses_QuestionId" ON "QuestionResponses" ("QuestionId");
CREATE INDEX IF NOT EXISTS "IX_QuestionResponses_AspNetUsersId" ON "QuestionResponses" ("AspNetUsersId");
CREATE INDEX IF NOT EXISTS "IX_QuestionResponses_ClientsId" ON "QuestionResponses" ("ClientsId");
CREATE INDEX IF NOT EXISTS "IX_QuestionResponses_ComponentType" ON "QuestionResponses" ("ComponentType");
CREATE INDEX IF NOT EXISTS "IX_QuestionResponses_IsDeleted" ON "QuestionResponses" ("IsDeleted") WHERE "IsDeleted" = FALSE;

-- Composite index for finding all responses for a user's question
CREATE INDEX IF NOT EXISTS "IX_QuestionResponses_UserQuestion" 
    ON "QuestionResponses" ("AspNetUsersId", "QuestionId", "IsDeleted") 
    WHERE "IsDeleted" = FALSE;

COMMENT ON TABLE "QuestionResponses" IS 'Stores user responses to question components';
COMMENT ON COLUMN "QuestionResponses"."ComponentType" IS 'Type of component (rating, checkbox_group, dropdown, comment)';
COMMENT ON COLUMN "QuestionResponses"."ComponentIndex" IS 'Index of the component within the question (0-based)';
COMMENT ON COLUMN "QuestionResponses"."ResponseValue" IS 'JSON value of the response (number for rating, array for checkbox, string for dropdown/comment)';

-- =============================================
-- 4. Migration Data (if needed)
-- =============================================
-- If you have existing questions, you may want to:
-- 1. Create a default survey for each client
-- 2. Link existing questions to the default survey
-- Uncomment and modify as needed:

-- INSERT INTO "Surveys" ("Title", "Description", "ClientsId", "IsActive", "CreatedAt", "IsDeleted")
-- SELECT 
--     'Default Survey',
--     'Default survey for existing questions',
--     "ClientsId",
--     TRUE,
--     NOW(),
--     FALSE
-- FROM (SELECT DISTINCT "ClientsId" FROM "Questions" WHERE "SurveyId" IS NULL) AS client_questions;

-- UPDATE "Questions" 
-- SET "SurveyId" = (
--     SELECT "Id" FROM "Surveys" 
--     WHERE "Surveys"."ClientsId" = "Questions"."ClientsId" 
--     LIMIT 1
-- )
-- WHERE "SurveyId" IS NULL;

-- =============================================
-- Verification
-- =============================================
SELECT 'Survey schema created successfully!' AS Status;

-- List all tables
SELECT 
    table_name,
    (SELECT COUNT(*) FROM information_schema.columns WHERE table_name = t.table_name) as column_count
FROM information_schema.tables t
WHERE table_schema = 'public' 
    AND table_type = 'BASE TABLE'
    AND table_name IN ('Surveys', 'Questions', 'QuestionResponses')
ORDER BY table_name;

-- Verify Surveys table structure
SELECT 
    column_name,
    data_type,
    is_nullable
FROM information_schema.columns 
WHERE table_schema = 'public' 
    AND table_name = 'Surveys'
ORDER BY ordinal_position;
