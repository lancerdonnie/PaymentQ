import React from 'react';

interface Props extends React.HTMLProps<HTMLInputElement> {
  onChange: (e: React.ChangeEvent<HTMLInputElement>) => void;
}

const Input = ({ value, className, ...props }: Props) => {
  return (
    <input
      value={value || ''}
      className={`focus:ring-indigo-500 focus:border-indigo-500 flex-1 block w-full rounded-md sm:text-sm border-gray-300 ${className}`}
      {...props}
    />
  );
};

export default Input;
