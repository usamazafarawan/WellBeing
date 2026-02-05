'use client';

import { useState, useEffect } from 'react';
import { useParams, useRouter } from 'next/navigation';
import { getSurveyWithQuestions, submitMultipleQuestionResponses } from '@/lib/api';
import { getOrCreateUserId } from '@/lib/auth';
import { SurveyWithQuestions, Question, QuestionResponse } from '@/types/question';
import QuestionRenderer from '@/components/questions/QuestionRenderer';
import { ChevronLeft, ChevronRight, Check, Brain, MessageSquare, HelpCircle, LogOut } from 'lucide-react';

export default function SurveyQuestionsPage() {
  const params = useParams();
  const router = useRouter();
  const surveyId = parseInt(params.id as string, 10);

  const [survey, setSurvey] = useState<SurveyWithQuestions | null>(null);
  const [currentQuestionIndex, setCurrentQuestionIndex] = useState(0);
  const [responses, setResponses] = useState<Record<number, Record<number, any>>>({});
  const [loading, setLoading] = useState(true);
  const [submitting, setSubmitting] = useState(false);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    loadSurvey();
  }, [surveyId]);

  const loadSurvey = async () => {
    try {
      setLoading(true);
      setError(null);
      const data = await getSurveyWithQuestions(surveyId);
      data.questions.sort((a, b) => a.displayOrder - b.displayOrder);
      setSurvey(data);
    } catch (err: any) {
      const errorMessage = err instanceof Error ? err.message : 'Failed to load survey. Please try again.';
      setError(errorMessage);
      console.error('Error loading survey:', err);
    } finally {
      setLoading(false);
    }
  };

  const handleResponseChange = (questionId: number, componentIndex: number, value: any) => {
    setResponses((prev) => ({
      ...prev,
      [questionId]: {
        ...prev[questionId],
        [componentIndex]: value,
      },
    }));
  };

  const isQuestionAnswered = (question: Question): boolean => {
    if (!question.questionConfig?.components) return true;

    const questionResponses = responses[question.id] || {};

    return question.questionConfig.components.every((component, index) => {
      if (!component.required) return true;
      const value = questionResponses[index];
      if (component.type === 'checkbox_group') {
        return Array.isArray(value) && value.length > 0;
      }
      return value !== null && value !== undefined && value !== '';
    });
  };

  const handleNext = () => {
    if (survey && currentQuestionIndex < survey.questions.length - 1) {
      setCurrentQuestionIndex(currentQuestionIndex + 1);
    }
  };

  const handlePrevious = () => {
    if (currentQuestionIndex > 0) {
      setCurrentQuestionIndex(currentQuestionIndex - 1);
    }
  };

  const handleSubmit = async () => {
    if (!survey) return;

    const unansweredRequired = survey.questions.filter(
      (q) => q.isRequired && !isQuestionAnswered(q)
    );

    if (unansweredRequired.length > 0) {
      setError('Please answer all required questions before submitting.');
      return;
    }

    try {
      setSubmitting(true);
      setError(null);

      const allResponses: QuestionResponse[] = [];

      survey.questions.forEach((question) => {
        if (!question.questionConfig?.components) return;

        const questionResponses = responses[question.id] || {};

        question.questionConfig.components.forEach((component, componentIndex) => {
          const value = questionResponses[componentIndex];
          if (value !== null && value !== undefined && value !== '') {
            allResponses.push({
              questionId: question.id,
              componentType: component.type,
              componentIndex,
              responseValue: value,
            });
          }
        });
      });

      const userId = await getOrCreateUserId(survey.clientsId);

      await submitMultipleQuestionResponses(allResponses, userId, survey.clientsId);

      router.push(`/survey/${surveyId}/thank-you`);
    } catch (err) {
      setError('Failed to submit responses. Please try again.');
      console.error(err);
    } finally {
      setSubmitting(false);
    }
  };

  if (loading) {
    return (
      <div className="min-h-screen flex items-center justify-center">
        <div className="text-center">
          <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-purple-600 mx-auto mb-4"></div>
          <p className="text-gray-600">Loading survey...</p>
        </div>
      </div>
    );
  }

  if (error && !survey) {
    return (
      <div className="min-h-screen flex items-center justify-center">
        <div className="text-center">
          <p className="text-red-600 mb-4">{error}</p>
          <button
            onClick={() => router.push(`/survey/${surveyId}`)}
            className="px-4 py-2 bg-purple-600 text-white rounded-lg hover:bg-purple-700"
          >
            Go Back
          </button>
        </div>
      </div>
    );
  }

  if (!survey || survey.questions.length === 0) {
    return (
      <div className="min-h-screen flex items-center justify-center">
        <div className="text-center">
          <p className="text-gray-600 mb-4">No questions found in this survey.</p>
          <button
            onClick={() => router.push(`/survey/${surveyId}`)}
            className="px-4 py-2 bg-purple-600 text-white rounded-lg hover:bg-purple-700"
          >
            Go Back
          </button>
        </div>
      </div>
    );
  }

  const currentQuestion = survey.questions[currentQuestionIndex];
  const isLastQuestion = currentQuestionIndex === survey.questions.length - 1;
  const progress = ((currentQuestionIndex + 1) / survey.questions.length) * 100;

  return (
    <div className="min-h-screen bg-white flex flex-col">
      <header className="bg-white border-b border-gray-200 px-6 py-4 flex-shrink-0">
        <div className="max-w-6xl mx-auto flex justify-between items-center">
          <div className="flex items-center gap-3">
            <div className="relative">
              <Brain className="w-8 h-8 text-purple-600" />
            </div>
            <div className="flex items-baseline gap-1">
              <span className="text-xl font-bold text-gray-800">CULTURE</span>
              <span className="text-xl font-bold text-purple-600">TELLIGENCE</span>
            </div>
          </div>

          <div className="flex items-center gap-2">
            <HelpCircle className="w-5 h-5 text-gray-500" />
            <span className="text-sm font-medium text-gray-700">
              {survey.clientsName || 'Innovate Co'}
            </span>
          </div>
        </div>
      </header>

      <main className="max-w-4xl mx-auto px-6 py-12 flex-1 flex flex-col">
        {error && (
          <div className="mb-6 p-4 bg-red-50 border border-red-200 rounded-lg text-red-700">
            {error}
          </div>
        )}

        <div className="flex justify-end mb-4">
          <button
            onClick={() => router.push(`/survey/${surveyId}`)}
            className="flex items-center gap-2 px-4 py-2 border border-gray-300 rounded-lg text-gray-700 hover:bg-gray-50 transition-colors text-sm font-medium bg-white"
          >
            <LogOut className="w-4 h-4" />
            Save & Exit
          </button>
        </div>

        <div className="bg-white rounded-lg border border-gray-200 shadow-sm p-10 mb-8 flex-1">
          <div className="text-sm text-gray-600 mb-6">
            Question {currentQuestionIndex + 1} of {survey.questions.length}
          </div>

          <h2 className="text-2xl font-bold text-gray-900 mb-8 leading-tight">
            {currentQuestion.questionText}
          </h2>

          <QuestionRenderer
            question={currentQuestion}
            responses={responses[currentQuestion.id] || {}}
            onResponseChange={(componentIndex, value) =>
              handleResponseChange(currentQuestion.id, componentIndex, value)
            }
          />
        </div>
      </main>

      <footer className="bg-white border-t border-gray-200 px-6 py-4 flex-shrink-0">
        <div className="max-w-6xl mx-auto flex items-center justify-between">
          <button
            onClick={handlePrevious}
            disabled={currentQuestionIndex === 0}
            className="flex items-center gap-2 px-6 py-3 text-gray-700 hover:text-gray-900 disabled:opacity-50 disabled:cursor-not-allowed transition-colors font-medium"
          >
            <ChevronLeft className="w-5 h-5" />
            Back
          </button>

          <div className="flex-1 mx-8">
            <div className="w-full bg-gray-200 rounded-full h-2">
              <div
                className="bg-gradient-to-r from-purple-600 to-blue-600 h-2 rounded-full transition-all duration-300"
                style={{ width: `${progress}%` }}
              ></div>
            </div>
          </div>

          {isLastQuestion ? (
            <button
              onClick={handleSubmit}
              disabled={submitting || !isQuestionAnswered(currentQuestion)}
              className="flex items-center gap-2 px-8 py-3 bg-gradient-to-r from-purple-600 to-blue-600 text-white rounded-lg hover:from-purple-700 hover:to-blue-700 disabled:opacity-50 disabled:cursor-not-allowed transition-all font-medium shadow-md"
            >
              {submitting ? (
                <>
                  <div className="animate-spin rounded-full h-5 w-5 border-2 border-white border-t-transparent"></div>
                  Submitting...
                </>
              ) : (
                <>
                  <Check className="w-5 h-5" />
                  Submit
                </>
              )}
            </button>
          ) : (
            <button
              onClick={handleNext}
              disabled={!isQuestionAnswered(currentQuestion)}
              className="flex items-center gap-2 px-8 py-3 bg-gradient-to-r from-purple-600 to-blue-600 text-white rounded-lg hover:from-purple-700 hover:to-blue-700 disabled:opacity-50 disabled:cursor-not-allowed transition-all font-medium shadow-md"
            >
              Next
              <ChevronRight className="w-5 h-5" />
            </button>
          )}
        </div>
      </footer>
    </div>
  );
}
