import { SurveyWithQuestions, QuestionResponse as QuestionResponseType } from '@/types/question';

export const API_BASE_URL = process.env.NEXT_PUBLIC_API_URL || 'http://localhost:5152/api';

if (typeof process !== 'undefined' && process.env.NODE_ENV === 'development') {
  process.env.NODE_TLS_REJECT_UNAUTHORIZED = '0';
}

export interface Survey {
  id: number;
  title: string;
  description?: string;
  clientsId: number;
  clientsName?: string;
  isActive: boolean;
  startDate?: string;
  endDate?: string;
  questionsCount: number;
  createdAt: string;
  modifiedAt?: string;
}

export async function getSurveyById(id: number): Promise<Survey> {
  try {
    const url = `${API_BASE_URL}/Surveys/${id}`;
    console.log(`[API] Fetching survey ${id} from ${url}`);
    
    const controller = new AbortController();
    const timeoutId = setTimeout(() => controller.abort(), 10000);
    
    const response = await fetch(url, {
      cache: 'no-store',
      headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json',
      },
      signal: controller.signal,
    });
    
    clearTimeout(timeoutId);

    if (!response.ok) {
      const errorText = await response.text();
      console.error(`[API] Error ${response.status} ${response.statusText}:`, errorText);
      throw new Error(`Failed to fetch survey: ${response.status} ${response.statusText}. ${errorText}`);
    }

    const data = await response.json();
    console.log('[API] Survey fetched successfully:', data.id, data.title);
    return data;
  } catch (error: any) {
    console.error('[API] Fetch error details:', {
      message: error?.message,
      cause: error?.cause,
      name: error?.name,
      stack: error?.stack,
    });
    
    if (error?.name === 'AbortError' || error?.name === 'TimeoutError') {
      throw new Error(
        `Request timeout: The API server at ${API_BASE_URL} did not respond within 10 seconds. ` +
        `Please check if the server is running and accessible.`
      );
    }
    
    if (error instanceof TypeError) {
      const errorMsg = error.message.toLowerCase();
      if (errorMsg.includes('fetch') || errorMsg.includes('network') || errorMsg.includes('connection')) {
        throw new Error(
          `Cannot connect to API at ${API_BASE_URL}. ` +
          `\n\nTroubleshooting steps:` +
          `\n1. Make sure the API server is running: cd Server/Wellbeing.API && dotnet run` +
          `\n2. Verify the server is listening on port 5152: http://localhost:5152/swagger` +
          `\n3. Check if the server was restarted after the Program.cs changes` +
          `\n4. Try accessing the API directly: ${API_BASE_URL}/Surveys/${id}`
        );
      }
    }
    
    throw error;
  }
}

export async function getAllSurveys(): Promise<Survey[]> {
  try {
    const response = await fetch(`${API_BASE_URL}/Surveys`, {
      cache: 'no-store',
      headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json',
      },
    });

    if (!response.ok) {
      const errorText = await response.text();
      throw new Error(`Failed to fetch surveys: ${response.status} ${response.statusText}. ${errorText}`);
    }

    return response.json();
  } catch (error) {
    if (error instanceof TypeError && error.message.includes('fetch')) {
      throw new Error(`Cannot connect to API at ${API_BASE_URL}. Make sure the server is running.`);
    }
    throw error;
  }
}

export async function getSurveyWithQuestions(id: number): Promise<SurveyWithQuestions> {
  try {
    const url = `${API_BASE_URL}/Surveys/${id}/questions`;
    console.log(`[API] Fetching survey ${id} with questions from ${url}`);
    
    const controller = new AbortController();
    const timeoutId = setTimeout(() => controller.abort(), 10000);
    
    const response = await fetch(url, {
      cache: 'no-store',
      headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json',
      },
      signal: controller.signal,
    });
    
    clearTimeout(timeoutId);

    if (!response.ok) {
      const errorText = await response.text();
      console.error(`[API] Error ${response.status} ${response.statusText}:`, errorText);
      throw new Error(`Failed to fetch survey with questions: ${response.status} ${response.statusText}. ${errorText}`);
    }

    const data = await response.json();
    console.log('[API] Survey with questions fetched successfully');
    
    if (data.questions && Array.isArray(data.questions)) {
      data.questions = data.questions.map((q: any) => {
        let questionConfig = null;
        
        if (q.questionConfig) {
          if (typeof q.questionConfig === 'string') {
            try {
              questionConfig = JSON.parse(q.questionConfig);
            } catch {
              questionConfig = null;
            }
          } else if (typeof q.questionConfig === 'object') {
            questionConfig = q.questionConfig;
          }
        }
        
        return {
          ...q,
          questionConfig,
        };
      });
    }

    return data;
  } catch (error: any) {
    console.error('[API] Fetch error details:', {
      message: error?.message,
      cause: error?.cause,
      name: error?.name,
    });
    
    if (error?.name === 'AbortError' || error?.name === 'TimeoutError') {
      throw new Error(
        `Request timeout: The API server at ${API_BASE_URL} did not respond within 10 seconds. ` +
        `Please check if the server is running and accessible.`
      );
    }
    
    if (error instanceof TypeError) {
      const errorMsg = error.message.toLowerCase();
      if (errorMsg.includes('fetch') || errorMsg.includes('network') || errorMsg.includes('connection') || errorMsg.includes('failed')) {
        throw new Error(
          `Cannot connect to API at ${API_BASE_URL}. ` +
          `\n\nTroubleshooting:` +
          `\n1. Verify the API server is running: Open http://localhost:5152/swagger in your browser` +
          `\n2. Check if the server was restarted after Program.cs changes` +
          `\n3. Try accessing the API directly: ${API_BASE_URL}/Surveys/${id}/questions` +
          `\n4. Check browser console for CORS or network errors`
        );
      }
    }
    
    throw error;
  }
}

export async function submitQuestionResponse(
  questionId: number,
  aspNetUsersId: string,
  clientsId: number,
  componentType: string,
  componentIndex: number,
  responseValue: string | number | string[]
): Promise<void> {
  let jsonValue: string;
  if (Array.isArray(responseValue)) {
    jsonValue = JSON.stringify(responseValue);
  } else if (typeof responseValue === 'string') {
    jsonValue = JSON.stringify(responseValue);
  } else {
    jsonValue = responseValue.toString();
  }

  try {
    const response = await fetch(`${API_BASE_URL}/QuestionResponses`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        'Accept': 'application/json',
      },
      body: JSON.stringify({
        questionId,
        aspNetUsersId,
        clientsId,
        componentType,
        componentIndex,
        responseValue: jsonValue,
      }),
    });

    if (!response.ok) {
      const errorText = await response.text();
      throw new Error(`Failed to submit response: ${response.status} ${response.statusText}. ${errorText}`);
    }
  } catch (error) {
    if (error instanceof TypeError && error.message.includes('fetch')) {
      throw new Error(`Cannot connect to API at ${API_BASE_URL}. Make sure the server is running.`);
    }
    throw error;
  }
}

export async function submitMultipleQuestionResponses(
  responses: QuestionResponseType[],
  aspNetUsersId: string,
  clientsId: number
): Promise<void> {
  const promises = responses.map((r) =>
    submitQuestionResponse(
      r.questionId,
      aspNetUsersId,
      clientsId,
      r.componentType,
      r.componentIndex,
      r.responseValue
    )
  );

  await Promise.all(promises);
}
