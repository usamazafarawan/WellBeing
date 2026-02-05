'use client';

import { Question, QuestionComponent } from '@/types/question';
import RatingComponent from './RatingComponent';
import CheckboxGroupComponent from './CheckboxGroupComponent';
import DropdownComponent from './DropdownComponent';
import CommentComponent from './CommentComponent';

interface QuestionRendererProps {
  question: Question;
  responses: Record<number, any>;
  onResponseChange: (componentIndex: number, value: any) => void;
}

export default function QuestionRenderer({
  question,
  responses,
  onResponseChange,
}: QuestionRendererProps) {
  if (!question.questionConfig || !question.questionConfig.components) {
    return null;
  }

  const components = question.questionConfig.components;

  return (
    <div className="space-y-6">
      {components.map((component, index) => {
        const responseKey = index;
        const currentValue = responses[responseKey] ?? getDefaultValue(component);

        const handleChange = (value: any) => {
          onResponseChange(index, value);
        };

        return (
          <div key={index} className="space-y-3">
            {component.label && (
              <h4 className="text-base font-medium text-gray-800">{component.label}</h4>
            )}
            {component.type === 'rating' && (
              <RatingComponent
                component={component}
                value={currentValue}
                onChange={handleChange}
                required={component.required}
              />
            )}
            {component.type === 'checkbox_group' && (
              <CheckboxGroupComponent
                component={component}
                value={currentValue}
                onChange={handleChange}
                required={component.required}
              />
            )}
            {component.type === 'dropdown' && (
              <DropdownComponent
                component={component}
                value={currentValue}
                onChange={handleChange}
                required={component.required}
              />
            )}
            {component.type === 'comment' && (
              <CommentComponent
                component={component}
                value={currentValue}
                onChange={handleChange}
                required={component.required}
              />
            )}
          </div>
        );
      })}
    </div>
  );
}

function getDefaultValue(component: QuestionComponent): any {
  switch (component.type) {
    case 'rating':
      return null;
    case 'checkbox_group':
      return [];
    case 'dropdown':
      return null;
    case 'comment':
      return '';
    default:
      return null;
  }
}
