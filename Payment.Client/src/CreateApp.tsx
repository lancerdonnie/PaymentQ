import { QueryFunctionContext } from 'react-query';
import { QueryClient, QueryClientProvider } from 'react-query';
import { Axios } from './Utils';
import { ToastContainer } from 'react-toastify';
import App from './App';

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

export default CreateApp;
