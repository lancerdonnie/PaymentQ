import React from 'react';

interface Props extends React.HTMLProps<HTMLTextAreaElement> {
  onChange: (e: React.ChangeEvent<HTMLTextAreaElement>) => void;
}

const Textarea = ({ className, ...props }: Props) => {
  return (
    <textarea
      className={`shadow-sm focus:ring-indigo-500 focus:border-indigo-500 mt-1 block w-full sm:text-sm border border-gray-300 rounded-md ${className}`}
      {...props}
    />
  );
};

export default Textarea;
