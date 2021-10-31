import { QueryFunctionContext, useMutation, useQuery } from 'react-query';
import { QueryClient, QueryClientProvider } from 'react-query';
import { Select, Input, Button, TextArea, Label, Toast } from 'Components';
import { useEffect, useState } from 'react';
import { Axios, formatCurrency } from 'Utils';
import apiMutate from 'Utils/api';
import { nanoid } from 'nanoid/non-secure';
import { ToastContainer } from 'react-toastify';

const queryClient = new QueryClient({
  defaultOptions: {
    queries: {
      queryFn: async ({ queryKey }: QueryFunctionContext) => {
        const { data } = await Axios.get(`/${queryKey[0]}`);
        return data;
      },
      refetchOnWindowFocus: true
    }
  }
});

function CreateApp() {
  return (
    <div className="App p-10">
      <ToastContainer />
      <QueryClientProvider client={queryClient} contextSharing>
        <App />
      </QueryClientProvider>
    </div>
  );
}

type Account = {
  accountNumber: string;
  balance: number;
};

type User = {
  name: string;
  account: Account;
  // bank: {item1: 101, item2: "Zenith"}
  email: string;
  // password: "ronaldo"
  // phone: "080000001"
};

type Balance = {
  accountNumber: 'string';
  balance: number;
};

type Temp = {
  id: string;
  accountNumber: string;
  amount: number | '';
  narration: string;
};

const App = () => {
  const [account, setAccount] = useState('');
  const [transactions, setTransactions] = useState<Temp[]>([]);
  const [temp, setTemp] = useState<Temp>({
    id: nanoid(),
    accountNumber: '',
    amount: '',
    narration: ''
  });

  const { data: users } = useQuery<{ data: User[] }>('users');

  const user = users?.data.find((e) => e.account.accountNumber === account);

  useEffect(() => {
    if (account) getBalance(account);
  }, [account]);

  const { mutate: getBalance, data: balance } = useMutation<
    { data: Balance },
    any,
    any
  >((accountNumber: string) => apiMutate('balance', { accountNumber }));

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
          {users?.data.map((user) => (
            <option key={user.email} value={user.account.accountNumber}>
              {user.name}
            </option>
          ))}
        </Select>

        {balance && (
          <div className="bg-purple-100 text-purple-600 flex items-center">
            <span className="p-2 px-3">Balance</span>
            <span className="bg-purple-500 text-white p-2 px-3 rounded-sm">
              {formatCurrency(balance?.data.balance)}
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
                {users?.data
                  .filter((e) => e.account.accountNumber !== account)
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

export default CreateApp;

export function Transactions({
  transactions,
  handleRemoveTransaction
}: {
  transactions: Temp[];
  handleRemoveTransaction: (trans: Temp) => void;
}) {
  if (!transactions?.length) return null;
  return (
    <div className="flex flex-col">
      <div className="-my-2 overflow-x-auto sm:-mx-6 lg:-mx-8">
        <div className="py-2 align-middle inline-block min-w-full sm:px-6 lg:px-8">
          <div className="shadow overflow-hidden border-b border-gray-200 sm:rounded-lg">
            <table className="min-w-full divide-y divide-gray-200">
              <thead className="bg-gray-50">
                <tr>
                  <th
                    scope="col"
                    className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider"
                  >
                    Account
                  </th>
                  <th
                    scope="col"
                    className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider"
                  >
                    Amount
                  </th>
                  <th
                    scope="col"
                    className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider"
                  >
                    Narration
                  </th>
                  <th scope="col" className="relative px-6 py-3">
                    <span className="sr-only">Remove</span>
                  </th>
                </tr>
              </thead>
              <tbody className="bg-white divide-y divide-gray-200">
                {transactions.map((trans) => (
                  <tr key={trans.id}>
                    <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">
                      {trans.accountNumber}
                    </td>
                    <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">
                      {trans.amount}
                    </td>
                    <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">
                      {trans.narration}
                    </td>
                    <td className="px-6 py-4 whitespace-nowrap text-right text-sm font-medium">
                      <a
                        href="#"
                        className="text-indigo-600 hover:text-indigo-900"
                        onClick={() => handleRemoveTransaction(trans)}
                      >
                        Remove
                      </a>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        </div>
      </div>
    </div>
  );
}
