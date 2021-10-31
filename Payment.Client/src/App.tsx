import { useMutation, useQuery } from 'react-query';
import { Select, Input, Button, TextArea, Label, Toast } from 'Components';
import { useEffect, useState } from 'react';
import { formatCurrency } from 'Utils';
import apiMutate from 'Utils/api';
import { nanoid } from 'nanoid/non-secure';
import type { Temp, Account, Balance, User } from './types';
import Transactions from 'Views/Transactions';

const App = () => {
  const [account, setAccount] = useState('');
  const [transactions, setTransactions] = useState<Temp[]>([]);
  const [temp, setTemp] = useState<Temp>({
    id: nanoid(),
    accountNumber: '',
    amount: '',
    narration: ''
  });

  const { data: users } = useQuery<User[]>('users');

  useEffect(() => {
    if (account) getBalance(account);
  }, [account]);

  const { mutate: getBalance, data: balance } = useMutation<Balance, any, any>(
    (accountNumber: string) => apiMutate('balance', { accountNumber })
  );

  const { mutate: pay, isLoading: paying } = useMutation(
    (data: any) => apiMutate('pay', data),
    {
      onSuccess: () => {
        getBalance(account);
        setTransactions([]);
        Toast({ msg: 'Payment made successfully', type: 'success' });
      }
    }
  );

  const handleAddTransaction = (e: React.FormEvent<HTMLFormElement>): void => {
    e.preventDefault();
    setTransactions([...transactions, temp]);
    resetTemp();
  };

  const handleRemoveTransaction = (trans: Temp) => {
    setTransactions(transactions.filter((e) => e.id !== trans.id));
  };

  const resetTemp = () => {
    setTemp({
      id: nanoid(),
      accountNumber: '',
      amount: '',
      narration: ''
    });
  };

  const handleSubmit = () => {
    if (!transactions.length) return Toast({ msg: 'Please add a transaction' });
    pay({
      paymentTransactions: transactions,
      sourceAccount: account
    });
  };

  return (
    <div>
      <div className="flex justify-between items-start">
        <Select
          value={account}
          onChange={(e) => {
            setAccount(e.target.value);
            setTransactions([]);
            resetTemp();
          }}
          className="max-w-lg"
        >
          <option value="">Select a user</option>
          {users?.map((user) => (
            <option key={user.email} value={user.account.accountNumber}>
              {user.name}
            </option>
          ))}
        </Select>

        {balance && (
          <div className="bg-purple-100 text-purple-600 flex items-center">
            <span className="p-2 px-3">Balance</span>
            <span className="bg-purple-500 text-white p-2 px-3 rounded-sm">
              {formatCurrency(balance?.balance)}
            </span>
          </div>
        )}
      </div>
      {account && (
        <div className="flex mt-10 ">
          <form onSubmit={handleAddTransaction} className="w-1/3 p-4">
            <div className="mt-4">
              <Label>Beneficiary</Label>
              <Select
                className="max-w-md"
                value={temp.accountNumber}
                onChange={(e) =>
                  setTemp({ ...temp, accountNumber: e.target.value })
                }
              >
                <option value={''}>Select</option>
                {users
                  ?.filter((e) => e.account.accountNumber !== account)
                  .map((user) => (
                    <option key={user.email} value={user.account.accountNumber}>
                      {user.name}
                    </option>
                  ))}
              </Select>
              <div className="mt-2">
                <Label>Amount</Label>

                <Input
                  type="number"
                  className="max-w-md "
                  value={temp.amount}
                  onChange={(e) =>
                    setTemp({ ...temp, amount: +e.target.value })
                  }
                />
              </div>
              <div className="mt-2">
                <Label>Narration</Label>
                <TextArea
                  value={temp.narration}
                  className="max-w-md"
                  onChange={(e) =>
                    setTemp({ ...temp, narration: e.target.value })
                  }
                />
              </div>
            </div>

            <div className="mt-3">
              <Button type="submit">Add Transaction</Button>
            </div>
          </form>
          <div className="w-2/3 p-4">
            <div className="mb-4 text-lg text-center">Transactions</div>
            <Transactions
              transactions={transactions}
              handleRemoveTransaction={handleRemoveTransaction}
            />
          </div>
        </div>
      )}
      {account && (
        <div className="mt-3 flex justify-center">
          <Button onClick={handleSubmit} loading={paying}>
            Pay
          </Button>
        </div>
      )}
    </div>
  );
};

export default App;
