'use client';

import { QuestionComponent } from '@/types/question';

interface CheckboxGroupComponentProps {
  component: QuestionComponent;
  value: string[];
  onChange: (value: string[]) => void;
  required?: boolean;
}

export default function CheckboxGroupComponent({
  component,
  value,
  onChange,
  required = false,
}: CheckboxGroupComponentProps) {
  const options = component.config.options || [];

  const handleChange = (optionValue: string, checked: boolean) => {
    if (checked) {
      onChange([...value, optionValue]);
    } else {
      onChange(value.filter((v) => v !== optionValue));
    }
  };

  return (
    <div className="space-y-3">
      {component.label && (
        <p className="text-sm font-medium text-gray-700 mb-2">{component.label}</p>
      )}
      <div className="space-y-2">
        {options.map((option) => (
          <label
            key={option.value}
            className="flex items-center gap-3 p-3 border border-gray-300 rounded-lg cursor-pointer hover:bg-gray-50 transition-colors"
          >
            <input
              type="checkbox"
              checked={value.includes(option.value)}
              onChange={(e) => handleChange(option.value, e.target.checked)}
              className="w-5 h-5 text-purple-600 focus:ring-purple-500 rounded"
            />
            <span className="text-gray-700">{option.label}</span>
          </label>
        ))}
      </div>
    </div>
  );
}
