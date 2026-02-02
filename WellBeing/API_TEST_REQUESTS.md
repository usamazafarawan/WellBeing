# API Test Request Bodies

This document contains example request bodies for testing all API endpoints in the WellBeing application.

## Base URL
```
http://localhost:5000/api
```
or
```
https://localhost:5001/api
```

---

## Survey APIs

### 1. Create Survey
**POST** `/api/Surveys`

```json
{
  "title": "Employee Wellbeing Survey 2024",
  "description": "Annual employee wellbeing assessment to measure satisfaction, work-life balance, and overall happiness",
  "clientsId": 1,
  "isActive": true,
  "startDate": "2024-01-01T00:00:00Z",
  "endDate": "2024-12-31T23:59:59Z"
}
```

**Minimal Request:**
```json
{
  "title": "Quick Survey",
  "clientsId": 1,
  "isActive": true
}
```

---

### 2. Update Survey
**PUT** `/api/Surveys/{id}`

```json
{
  "id": 1,
  "title": "Updated Employee Wellbeing Survey 2024",
  "description": "Updated description for the survey",
  "isActive": false,
  "startDate": "2024-02-01T00:00:00Z",
  "endDate": "2024-11-30T23:59:59Z"
}
```

---

### 3. Get All Surveys
**GET** `/api/Surveys`

**Query Parameters:**
- `clientsId` (optional): Filter by client ID
- `isActive` (optional): Filter by active status (true/false)

**Examples:**
```
GET /api/Surveys
GET /api/Surveys?clientsId=1
GET /api/Surveys?isActive=true
GET /api/Surveys?clientsId=1&isActive=true
```

---

### 4. Get Survey by ID
**GET** `/api/Surveys/{id}`

```
GET /api/Surveys/1
```

---

### 5. Get Survey with Questions
**GET** `/api/Surveys/{id}/questions`

```
GET /api/Surveys/1/questions
```

---

### 6. Delete Survey
**DELETE** `/api/Surveys/{id}`

```
DELETE /api/Surveys/1
```

---

## Question APIs

### 7. Create Question - Simple Rating
**POST** `/api/Questions`

```json
{
  "questionText": "I feel supported by my direct manager to maintain a healthy work-life balance.",
  "questionType": "rating",
  "surveyId": 1,
  "clientsId": 1,
  "isRequired": true,
  "displayOrder": 1,
  "questionConfig": "{\"components\":[{\"type\":\"rating\",\"required\":true,\"config\":{\"min\":1,\"max\":5,\"labels\":[\"Strongly Disagree\",\"Disagree\",\"Neutral\",\"Agree\",\"Strongly Agree\"]}}]}"
}
```

---

### 8. Create Question - Rating + Checkbox + Comment
**POST** `/api/Questions`

```json
{
  "questionText": "I have the tools and resources I need to do my job effectively.",
  "questionType": "mixed",
  "surveyId": 1,
  "clientsId": 1,
  "isRequired": true,
  "displayOrder": 5,
  "questionConfig": "{\"components\":[{\"type\":\"rating\",\"required\":true,\"config\":{\"min\":1,\"max\":5,\"labels\":[\"Strongly Disagree\",\"Disagree\",\"Neutral\",\"Agree\",\"Strongly Agree\"]}},{\"type\":\"checkbox_group\",\"required\":false,\"label\":\"What is currently missing? (Select all that apply)\",\"config\":{\"options\":[{\"label\":\"Software/Applications\",\"value\":\"software\"},{\"label\":\"Hardware/Equipment\",\"value\":\"hardware\"},{\"label\":\"Budget/Funding\",\"value\":\"budget\"}]}},{\"type\":\"comment\",\"required\":false,\"config\":{\"maxLength\":1000,\"placeholder\":\"Tell us more about why you chose this...\"}}]}"
}
```

---

### 9. Create Question - Dropdown
**POST** `/api/Questions`

```json
{
  "questionText": "How would you prefer to receive feedback?",
  "questionType": "dropdown",
  "surveyId": 1,
  "clientsId": 1,
  "isRequired": true,
  "displayOrder": 3,
  "questionConfig": "{\"components\":[{\"type\":\"dropdown\",\"required\":true,\"config\":{\"options\":[{\"label\":\"Email\",\"value\":\"email\"},{\"label\":\"In-person meeting\",\"value\":\"meeting\"},{\"label\":\"Written report\",\"value\":\"report\"},{\"label\":\"Phone call\",\"value\":\"phone\"}]}}]}"
}
```

---

### 10. Create Question - Multiple Checkboxes
**POST** `/api/Questions`

```json
{
  "questionText": "Which of the following benefits are most important to you? (Select all that apply)",
  "questionType": "checkbox_group",
  "surveyId": 1,
  "clientsId": 1,
  "isRequired": false,
  "displayOrder": 4,
  "questionConfig": "{\"components\":[{\"type\":\"checkbox_group\",\"required\":false,\"config\":{\"options\":[{\"label\":\"Health Insurance\",\"value\":\"health\"},{\"label\":\"Retirement Plan\",\"value\":\"retirement\"},{\"label\":\"Paid Time Off\",\"value\":\"pto\"},{\"label\":\"Flexible Work Hours\",\"value\":\"flexible\"},{\"label\":\"Professional Development\",\"value\":\"development\"}]}}]}"
}
```

---

### 11. Create Question - Comment Only
**POST** `/api/Questions`

```json
{
  "questionText": "Please share any additional comments or suggestions about your work environment.",
  "questionType": "comment",
  "surveyId": 1,
  "clientsId": 1,
  "isRequired": false,
  "displayOrder": 10,
  "questionConfig": "{\"components\":[{\"type\":\"comment\",\"required\":false,\"config\":{\"maxLength\":2000,\"placeholder\":\"Enter your comments here...\"}}]}"
}
```

---

### 12. Create Question - With Wellbeing Dimensions (Optional)
**POST** `/api/Questions`

```json
{
  "questionText": "I feel mentally and emotionally supported at work.",
  "questionType": "rating",
  "surveyId": 1,
  "clientsId": 1,
  "isRequired": true,
  "displayOrder": 2,
  "wellbeingDimensionId": 1,
  "wellbeingSubDimensionId": 1,
  "questionConfig": "{\"components\":[{\"type\":\"rating\",\"required\":true,\"config\":{\"min\":1,\"max\":5,\"labels\":[\"Strongly Disagree\",\"Disagree\",\"Neutral\",\"Agree\",\"Strongly Agree\"]}}]}"
}
```

---

### 13. Update Question
**PUT** `/api/Questions/{id}`

```json
{
  "id": 1,
  "questionText": "Updated question text here",
  "questionType": "rating",
  "surveyId": 1,
  "isRequired": false,
  "displayOrder": 2,
  "questionConfig": "{\"components\":[{\"type\":\"rating\",\"required\":false,\"config\":{\"min\":1,\"max\":10}}]}"
}
```

---

### 14. Get All Questions
**GET** `/api/Questions`

**Query Parameters:**
- `surveyId` (optional): Filter by survey ID
- `clientsId` (optional): Filter by client ID

**Examples:**
```
GET /api/Questions
GET /api/Questions?surveyId=1
GET /api/Questions?clientsId=1
GET /api/Questions?surveyId=1&clientsId=1
```

---

### 15. Get Questions by Survey
**GET** `/api/Questions/survey/{surveyId}`

```
GET /api/Questions/survey/1
```

---

### 16. Get Question by ID
**GET** `/api/Questions/{id}`

```
GET /api/Questions/1
```

---

### 17. Delete Question
**DELETE** `/api/Questions/{id}`

```
DELETE /api/Questions/1
```

---

## QuestionResponse APIs

### 18. Submit Response - Rating
**POST** `/api/QuestionResponses`

```json
{
  "questionId": 1,
  "aspNetUsersId": "123e4567-e89b-12d3-a456-426614174000",
  "clientsId": 1,
  "componentType": "rating",
  "componentIndex": 0,
  "responseValue": "5"
}
```

---

### 19. Submit Response - Checkbox Group
**POST** `/api/QuestionResponses`

```json
{
  "questionId": 2,
  "aspNetUsersId": "123e4567-e89b-12d3-a456-426614174000",
  "clientsId": 1,
  "componentType": "checkbox_group",
  "componentIndex": 0,
  "responseValue": "[\"software\", \"hardware\"]"
}
```

---

### 20. Submit Response - Dropdown
**POST** `/api/QuestionResponses`

```json
{
  "questionId": 3,
  "aspNetUsersId": "123e4567-e89b-12d3-a456-426614174000",
  "clientsId": 1,
  "componentType": "dropdown",
  "componentIndex": 0,
  "responseValue": "\"email\""
}
```

---

### 21. Submit Response - Comment
**POST** `/api/QuestionResponses`

```json
{
  "questionId": 4,
  "aspNetUsersId": "123e4567-e89b-12d3-a456-426614174000",
  "clientsId": 1,
  "componentType": "comment",
  "componentIndex": 0,
  "responseValue": "\"Great support team and excellent work environment!\""
}
```

---

### 22. Submit Response - Multiple Components (Rating + Checkbox)
**POST** `/api/QuestionResponses`

**First Component (Rating):**
```json
{
  "questionId": 5,
  "aspNetUsersId": "123e4567-e89b-12d3-a456-426614174000",
  "clientsId": 1,
  "componentType": "rating",
  "componentIndex": 0,
  "responseValue": "4"
}
```

**Second Component (Checkbox Group):**
```json
{
  "questionId": 5,
  "aspNetUsersId": "123e4567-e89b-12d3-a456-426614174000",
  "clientsId": 1,
  "componentType": "checkbox_group",
  "componentIndex": 1,
  "responseValue": "[\"software\", \"budget\"]"
}
```

**Third Component (Comment):**
```json
{
  "questionId": 5,
  "aspNetUsersId": "123e4567-e89b-12d3-a456-426614174000",
  "clientsId": 1,
  "componentType": "comment",
  "componentIndex": 2,
  "responseValue": "\"We need better project management tools.\""
}
```

---

### 23. Get Question Responses
**GET** `/api/QuestionResponses`

**Query Parameters:**
- `questionId` (optional): Filter by question ID
- `aspNetUsersId` (optional): Filter by user ID (GUID)
- `clientsId` (optional): Filter by client ID

**Examples:**
```
GET /api/QuestionResponses
GET /api/QuestionResponses?questionId=1
GET /api/QuestionResponses?aspNetUsersId=123e4567-e89b-12d3-a456-426614174000
GET /api/QuestionResponses?clientsId=1
GET /api/QuestionResponses?questionId=1&aspNetUsersId=123e4567-e89b-12d3-a456-426614174000
```

---

## QuestionConfig Examples (Formatted for Readability)

### Simple Rating Component
```json
{
  "components": [
    {
      "type": "rating",
      "required": true,
      "config": {
        "min": 1,
        "max": 5,
        "labels": ["Strongly Disagree", "Disagree", "Neutral", "Agree", "Strongly Agree"]
      }
    }
  ]
}
```

### Rating with 1-10 Scale
```json
{
  "components": [
    {
      "type": "rating",
      "required": true,
      "config": {
        "min": 1,
        "max": 10
      }
    }
  ]
}
```

### Checkbox Group
```json
{
  "components": [
    {
      "type": "checkbox_group",
      "required": false,
      "label": "Select all that apply",
      "config": {
        "options": [
          {"label": "Option 1", "value": "opt1"},
          {"label": "Option 2", "value": "opt2"},
          {"label": "Option 3", "value": "opt3"}
        ]
      }
    }
  ]
}
```

### Dropdown
```json
{
  "components": [
    {
      "type": "dropdown",
      "required": true,
      "config": {
        "options": [
          {"label": "Option 1", "value": "opt1"},
          {"label": "Option 2", "value": "opt2"},
          {"label": "Option 3", "value": "opt3"}
        ]
      }
    }
  ]
}
```

### Comment Box
```json
{
  "components": [
    {
      "type": "comment",
      "required": false,
      "config": {
        "maxLength": 1000,
        "placeholder": "Enter your comments here..."
      }
    }
  ]
}
```

### Multiple Components (Rating + Checkbox + Comment)
```json
{
  "components": [
    {
      "type": "rating",
      "required": true,
      "config": {
        "min": 1,
        "max": 5,
        "labels": ["Poor", "Fair", "Good", "Very Good", "Excellent"]
      }
    },
    {
      "type": "checkbox_group",
      "required": false,
      "label": "What could be improved?",
      "config": {
        "options": [
          {"label": "Communication", "value": "communication"},
          {"label": "Resources", "value": "resources"},
          {"label": "Support", "value": "support"}
        ]
      }
    },
    {
      "type": "comment",
      "required": false,
      "config": {
        "maxLength": 500,
        "placeholder": "Additional feedback..."
      }
    }
  ]
}
```

---

## Response Value Formats

### Rating Response
```json
"5"
```
or
```json
"10"
```
(Number as JSON string)

### Checkbox Group Response
```json
"[\"value1\", \"value2\", \"value3\"]"
```
(Array as JSON string)

### Dropdown Response
```json
"\"selected_value\""
```
(String as JSON string)

### Comment Response
```json
"\"This is my comment text.\""
```
(String as JSON string)

---

## Testing Workflow Example

1. **Create a Survey:**
   ```json
   POST /api/Surveys
   {
     "title": "Employee Satisfaction Survey",
     "clientsId": 1,
     "isActive": true
   }
   ```
   Response: `{ "id": 1, ... }`

2. **Create Questions for the Survey:**
   ```json
   POST /api/Questions
   {
     "questionText": "How satisfied are you with your current role?",
     "questionType": "rating",
     "surveyId": 1,
     "clientsId": 1,
     "isRequired": true,
     "displayOrder": 1,
     "questionConfig": "{\"components\":[{\"type\":\"rating\",\"required\":true,\"config\":{\"min\":1,\"max\":5}}]}"
   }
   ```

3. **Get Survey with Questions:**
   ```
   GET /api/Surveys/1/questions
   ```

4. **Submit Response:**
   ```json
   POST /api/QuestionResponses
   {
     "questionId": 1,
     "aspNetUsersId": "user-guid-here",
     "clientsId": 1,
     "componentType": "rating",
     "componentIndex": 0,
     "responseValue": "4"
   }
   ```

5. **Get User's Responses:**
   ```
   GET /api/QuestionResponses?aspNetUsersId=user-guid-here
   ```

---

## Notes

- All dates should be in ISO 8601 format (e.g., `2024-01-01T00:00:00Z`)
- `aspNetUsersId` should be a valid GUID
- `questionConfig` must be a valid JSON string (escape quotes when sending)
- `responseValue` must be valid JSON matching the component type
- `componentIndex` starts at 0 for the first component in a question
- When updating a response, use the same `questionId`, `aspNetUsersId`, and `componentIndex` to update the existing response
