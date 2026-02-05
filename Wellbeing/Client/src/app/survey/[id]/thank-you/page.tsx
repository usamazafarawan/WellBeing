'use client';

import { useParams, useRouter } from 'next/navigation';
import { CheckCircle } from 'lucide-react';

export default function ThankYouPage() {
  const params = useParams();
  const router = useRouter();
  const surveyId = params.id as string;

  return (
    <div className="min-h-screen bg-gray-50 flex items-center justify-center px-6">
      <div className="max-w-2xl w-full bg-white rounded-lg shadow-lg p-12 text-center">
        <div className="mb-6 flex justify-center">
          <div className="w-20 h-20 bg-green-100 rounded-full flex items-center justify-center">
            <CheckCircle className="w-12 h-12 text-green-600" />
          </div>
        </div>
        <h1 className="text-3xl font-bold text-gray-900 mb-4">
          Thank You!
        </h1>
        <p className="text-lg text-gray-600 mb-8">
          Your responses have been submitted successfully. We appreciate you taking the time to share your feedback.
        </p>
        <button
          onClick={() => router.push('/')}
          className="px-8 py-3 bg-purple-600 text-white rounded-lg hover:bg-purple-700 transition-colors"
        >
          Return to Home
        </button>
      </div>
    </div>
  );
}
