import React, { Children } from 'react';

<style>
  {/* .top-100 {top: 100%}
    .bottom-100 {bottom: 100%}
    .max-h-select {
        max-height: 300px;
    } */}
</style>;

interface Props extends React.HTMLProps<HTMLSelectElement> {
  children: React.ReactNode;
  onChange: (e: React.ChangeEvent<HTMLSelectElement>) => void;
}

const Select = ({ children, className, ...props }: Props) => {
  return (
    <select
      className={`mt-1 block w-full py-2 px-3 border border-gray-300 bg-white rounded-md shadow-sm focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm ${className}`}
      {...props}
    >
      {children}
    </select>
  );
};

export default Select;
