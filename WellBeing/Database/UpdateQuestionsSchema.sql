
-- Add new columns to Questions table
ALTER TABLE "Questions" 
    ADD COLUMN IF NOT EXISTS "QuestionConfig" JSONB NULL,
    ADD COLUMN IF NOT EXISTS "IsRequired" BOOLEAN NOT NULL DEFAULT TRUE,
    ADD COLUMN IF NOT EXISTS "DisplayOrder" INTEGER NOT NULL DEFAULT 0;

-- Update QuestionText length to support longer questions
ALTER TABLE "Questions" 
    ALTER COLUMN "QuestionText" TYPE VARCHAR(2000);

-- Create index for display order
CREATE INDEX IF NOT EXISTS "IX_Questions_DisplayOrder" ON "Questions" ("DisplayOrder", "ClientsId");

-- =============================================
-- Create QuestionResponses Table
-- =============================================
-- This table stores user responses to questions
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
COMMENT ON COLUMN "Questions"."QuestionConfig" IS 'JSON structure defining question components (rating, checkbox_group, dropdown, comment)';
COMMENT ON COLUMN "Questions"."IsRequired" IS 'Whether the question must be answered';
COMMENT ON COLUMN "Questions"."DisplayOrder" IS 'Order in which questions are displayed';
COMMENT ON COLUMN "QuestionResponses"."ComponentType" IS 'Type of component (rating, checkbox_group, dropdown, comment)';
COMMENT ON COLUMN "QuestionResponses"."ComponentIndex" IS 'Index of the component within the question (0-based)';
COMMENT ON COLUMN "QuestionResponses"."ResponseValue" IS 'JSON value of the response (number for rating, array for checkbox, string for dropdown/comment)';

-- Display success message
SELECT 'Questions table updated successfully with component support!' AS Status;
