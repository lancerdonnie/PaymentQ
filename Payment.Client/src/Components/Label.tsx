import React from 'react';

interface Props extends React.LabelHTMLAttributes<HTMLLabelElement> {
  children: React.ReactNode;
}

const Label = ({ children, ...props }: Props) => {
  return (
    <label
      htmlFor="about"
      className="block text-sm font-medium text-gray-700"
      {...props}
    >
      {children}
    </label>
  );
};

export default Label;
