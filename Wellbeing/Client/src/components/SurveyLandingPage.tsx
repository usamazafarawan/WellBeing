'use client';

import { Clock, ShieldCheck, Lightbulb, Brain, MessageSquare } from 'lucide-react';
import { Survey } from '@/lib/api';
import { useRouter } from 'next/navigation';

interface SurveyLandingPageProps {
  survey: Survey;
  userName?: string;
}

export default function SurveyLandingPage({ survey, userName = 'Alex' }: SurveyLandingPageProps) {
  const router = useRouter();

  const handleStartSurvey = () => {
    router.push(`/survey/${survey.id}/questions`);
  };

  return (
    <div className="min-h-screen bg-gradient-to-b from-gray-50 to-white">
      <header className="flex justify-between items-center p-6 border-b border-gray-200 bg-white">
        <div className="flex items-center gap-3">
          <Brain className="w-8 h-8 text-purple-600" />
          <div className="flex items-baseline gap-1">
            <span className="text-2xl font-bold text-gray-800">CULTURE</span>
            <span className="text-2xl font-bold text-purple-600">TELLIGENCE</span>
          </div>
        </div>
        <div className="flex items-center gap-4">
          <div className="flex items-center gap-2">
            <MessageSquare className="w-5 h-5 text-gray-500" />
            <span className="text-sm font-medium text-gray-700">
              {survey.clientsName || 'Innovate Co'}
            </span>
          </div>
          <select className="px-3 py-1.5 border border-gray-300 rounded-md text-sm text-gray-700 bg-white focus:outline-none focus:ring-2 focus:ring-purple-500">
            <option value="en">English</option>
            <option value="es">Spanish</option>
            <option value="fr">French</option>
          </select>
        </div>
      </header>

      <main className="max-w-4xl mx-auto px-6 py-12">
        <div className="text-center mb-12">
          <h2 className="text-5xl font-bold text-gray-900 mb-4">
            Welcome {userName}.
          </h2>
          <p className="text-xl text-gray-600">
            Thank you for taking a moment to share your perspective.
          </p>
        </div>

        <div className="grid md:grid-cols-3 gap-6 mb-12">
          <div className="bg-white rounded-lg shadow-md p-6 border border-gray-100">
            <div className="flex items-center gap-3 mb-4">
              <div className="p-2 bg-purple-100 rounded-lg">
                <Clock className="w-6 h-6 text-purple-600" />
              </div>
              <h3 className="text-lg font-semibold text-gray-900">~8 Minutes</h3>
            </div>
            <p className="text-gray-600 text-sm leading-relaxed">
              Quick to complete, with your progress saved at every step.
            </p>
          </div>

          <div className="bg-white rounded-lg shadow-md p-6 border border-gray-100">
            <div className="flex items-center gap-3 mb-4">
              <div className="p-2 bg-purple-100 rounded-lg">
                <ShieldCheck className="w-6 h-6 text-purple-600" />
              </div>
              <h3 className="text-lg font-semibold text-gray-900">100% Confidential</h3>
            </div>
            <p className="text-gray-600 text-sm leading-relaxed">
              Your name is used here only for personalization. Your individual responses are encrypted and aggregated; no one at {survey.clientsName || 'SurveyCompass Inc.'} will see your specific answers.
            </p>
          </div>

          <div className="bg-white rounded-lg shadow-md p-6 border border-gray-100">
            <div className="flex items-center gap-3 mb-4">
              <div className="p-2 bg-purple-100 rounded-lg">
                <Lightbulb className="w-6 h-6 text-purple-600" />
              </div>
              <h3 className="text-lg font-semibold text-gray-900">Be Authentic</h3>
            </div>
            <p className="text-gray-600 text-sm leading-relaxed">
              Your honest feedback is the only way we can make meaningful improvements.
            </p>
          </div>
        </div>

        <div className="text-center mb-8">
          <p className="text-sm text-gray-600 mb-2">
            By clicking &apos;Start&apos;, you agree to participate in this engagement study. Results are used to identify company-wide trends and action area themes.
          </p>
          <a
            href="#"
            className="text-sm text-purple-600 hover:text-purple-700 underline"
          >
            Read our Data Protection & Privacy Policy
          </a>
        </div>

        <div className="flex justify-center">
          <button
            onClick={handleStartSurvey}
            className="bg-purple-600 hover:bg-purple-700 text-white font-semibold py-4 px-12 rounded-lg text-lg shadow-lg transition-colors duration-200 transform hover:scale-105"
          >
            START SURVEY
          </button>
        </div>
      </main>
    </div>
  );
}
