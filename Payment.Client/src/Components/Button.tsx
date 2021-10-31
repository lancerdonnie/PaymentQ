import React from 'react';

interface Props extends React.ButtonHTMLAttributes<HTMLButtonElement> {
  children: React.ReactNode;
  loading?: boolean;
}

const Button = ({ children, loading, ...props }: Props) => {
  const _loading = loading ? 'loading' : '';
  return (
    <button
      {...props}
      className={`h-[42px] flex items-center justify-center bg-purple-200 hover:bg-purple-300 text-purple-800  px-4 rounded shadow relative ${_loading}`}
    >
      {children}
    </button>
  );
};

export default Button;
