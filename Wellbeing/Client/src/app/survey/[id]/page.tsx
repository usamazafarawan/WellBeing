import { getSurveyById } from '@/lib/api';
import SurveyLandingPage from '@/components/SurveyLandingPage';
import { notFound } from 'next/navigation';

interface SurveyPageProps {
  params: Promise<{
    id: string;
  }>;
}

export default async function SurveyPage({ params }: SurveyPageProps) {
  const { id } = await params;
  const surveyId = parseInt(id, 10);

  if (isNaN(surveyId)) {
    notFound();
  }

  try {
    const survey = await getSurveyById(surveyId);
    
    if (!survey.isActive) {
      return (
        <div className="min-h-screen flex items-center justify-center">
          <div className="text-center">
            <h1 className="text-2xl font-bold text-gray-900 mb-4">Survey Not Available</h1>
            <p className="text-gray-600">This survey is currently inactive.</p>
          </div>
        </div>
      );
    }

    return <SurveyLandingPage survey={survey} />;
  } catch (error) {
    console.error('Error fetching survey:', error);
    return (
      <div className="min-h-screen flex items-center justify-center">
        <div className="text-center">
          <h1 className="text-2xl font-bold text-gray-900 mb-4">Error Loading Survey</h1>
          <p className="text-gray-600 mb-4">
            {error instanceof Error ? error.message : 'Failed to load survey. Please check:'}
          </p>
          <ul className="text-sm text-gray-500 text-left inline-block">
            <li>• Is the API server running? (Check port 5152 for HTTP or 7027 for HTTPS)</li>
            <li>• Try accessing: <a href="http://localhost:5152/api/Surveys/{surveyId}" target="_blank" className="text-blue-600 underline">http://localhost:5152/api/Surveys/{surveyId}</a></li>
            <li>• If using HTTPS, visit https://localhost:7027/swagger first to accept the certificate</li>
            <li>• Does survey ID {surveyId} exist in the database?</li>
          </ul>
        </div>
      </div>
    );
  }
}

export async function generateMetadata({ params }: SurveyPageProps) {
  const { id } = await params;
  const surveyId = parseInt(id, 10);

  try {
    const survey = await getSurveyById(surveyId);
    return {
      title: `${survey.title} - Wellbeing Survey`,
      description: survey.description || 'Participate in our wellbeing survey',
    };
  } catch {
    return {
      title: 'Survey Not Found',
    };
  }
}
