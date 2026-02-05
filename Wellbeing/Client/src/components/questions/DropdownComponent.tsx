'use client';

import { QuestionComponent } from '@/types/question';

interface DropdownComponentProps {
  component: QuestionComponent;
  value: string | null;
  onChange: (value: string) => void;
  required?: boolean;
}

export default function DropdownComponent({
  component,
  value,
  onChange,
  required = false,
}: DropdownComponentProps) {
  const options = component.config.options || [];

  return (
    <div className="space-y-2">
      <select
        value={value || ''}
        onChange={(e) => onChange(e.target.value)}
        required={required}
        className="w-full px-4 py-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-purple-500 focus:border-purple-500 text-gray-700 bg-white"
      >
        <option value="">Select an option...</option>
        {options.map((option) => (
          <option key={option.value} value={option.value}>
            {option.label}
          </option>
        ))}
      </select>
    </div>
  );
}
