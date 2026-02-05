export interface QuestionComponent {
  type: 'rating' | 'checkbox_group' | 'dropdown' | 'comment';
  required: boolean;
  label?: string;
  config: {
    min?: number;
    max?: number;
    labels?: string[];
    options?: { label: string; value: string }[];
    maxLength?: number;
    placeholder?: string;
  };
}

export interface QuestionConfig {
  components: QuestionComponent[];
}

export interface Question {
  id: number;
  questionText: string;
  questionType: string | null;
  surveyId: number;
  clientsId: number;
  questionConfig: QuestionConfig | null;
  isRequired: boolean;
  displayOrder: number;
  wellbeingDimensionId?: number | null;
  wellbeingSubDimensionId?: number | null;
}

export interface SurveyWithQuestions {
  id: number;
  title: string;
  description?: string;
  clientsId: number;
  clientsName?: string;
  isActive: boolean;
  questions: Question[];
}

export interface QuestionResponse {
  questionId: number;
  componentType: string;
  componentIndex: number;
  responseValue: string | number | string[];
}
