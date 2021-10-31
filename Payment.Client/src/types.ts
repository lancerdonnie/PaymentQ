export type Temp = {
  id: string;
  accountNumber: string;
  amount: number | '';
  narration: string;
};

export type Account = {
  accountNumber: string;
  balance: number;
};

export type User = {
  name: string;
  account: Account;
  email: string;
};

export type Balance = {
  accountNumber: 'string';
  balance: number;
};
