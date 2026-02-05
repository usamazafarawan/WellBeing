'use client';

import { QuestionComponent } from '@/types/question';

interface RatingComponentProps {
  component: QuestionComponent;
  value: number | null;
  onChange: (value: number) => void;
  required?: boolean;
}

export default function RatingComponent({
  component,
  value,
  onChange,
  required = false,
}: RatingComponentProps) {
  const min = component.config.min || 1;
  const max = component.config.max || 5;
  const labels = component.config.labels || [];

  return (
    <div className="space-y-4">
      {labels.length > 0 ? (
        <div className="flex gap-3 flex-wrap">
          {labels.map((label, index) => {
            const ratingValue = min + index;
            const isSelected = value === ratingValue;
            return (
              <button
                key={ratingValue}
                type="button"
                onClick={() => onChange(ratingValue)}
                className={`
                  px-6 py-4 rounded-lg font-medium text-sm transition-all
                  border-2 min-w-[140px]
                  ${
                    isSelected
                      ? 'bg-blue-600 border-blue-600 text-white shadow-md'
                      : 'bg-white border-gray-300 text-gray-700 hover:border-gray-400 hover:bg-gray-50'
                  }
                `}
              >
                <span className="font-semibold">{ratingValue}</span>{' '}
                <span className="ml-1">{label}</span>
              </button>
            );
          })}
        </div>
      ) : (
        <div className="flex items-center gap-3">
          <div className="flex gap-2">
            {Array.from({ length: max - min + 1 }, (_, i) => {
              const ratingValue = min + i;
              const isSelected = value === ratingValue;
              return (
                <button
                  key={ratingValue}
                  type="button"
                  onClick={() => onChange(ratingValue)}
                  className={`
                    w-14 h-14 rounded-lg font-bold text-lg transition-all
                    ${
                      isSelected
                        ? 'bg-blue-600 border-2 border-blue-600 text-white shadow-md'
                        : 'bg-gray-100 border-2 border-gray-300 text-gray-700 hover:bg-gray-200 hover:border-gray-400'
                    }
                  `}
                >
                  {ratingValue}
                </button>
              );
            })}
          </div>
        </div>
      )}
    </div>
  );
}
