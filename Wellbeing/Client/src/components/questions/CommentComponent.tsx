'use client';

import { QuestionComponent } from '@/types/question';

interface CommentComponentProps {
  component: QuestionComponent;
  value: string;
  onChange: (value: string) => void;
  required?: boolean;
}

export default function CommentComponent({
  component,
  value,
  onChange,
  required = false,
}: CommentComponentProps) {
  const maxLength = component.config.maxLength || 2000;
  const placeholder = component.config.placeholder || 'Enter your comments here...';

  return (
    <div className="space-y-2">
      <textarea
        value={value}
        onChange={(e) => {
          if (e.target.value.length <= maxLength) {
            onChange(e.target.value);
          }
        }}
        required={required}
        placeholder={placeholder}
        rows={6}
        className="w-full px-4 py-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-purple-500 focus:border-purple-500 text-gray-700 resize-none"
      />
      <div className="text-right text-sm text-gray-500">
        {value.length} / {maxLength} characters
      </div>
    </div>
  );
}
