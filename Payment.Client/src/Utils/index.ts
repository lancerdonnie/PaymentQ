import axios from 'axios';
import { Toast } from 'Components';

const Axios = axios.create({});
Axios.defaults.baseURL = import.meta.env.VITE_BASE_URL as string;
Axios.defaults.headers.post['Content-Type'] = 'application/json';
Axios.interceptors.response.use(
  (response) => {
    return response;
  },
  (error) => {
    if (error.response?.status === 404) {
      Toast({
        type: 'error',
        msg: error.response?.data
      });
    } else if (error.response?.data?.errors) {
      try {
        const obj = Object.keys(error.response.data.errors)[0];
        const err = error.response.data.errors[obj][0];
        if (err) {
          Toast({
            type: 'error',
            msg: (obj.split('.')[1] ?? obj) + '=>' + err.split('. Path')[0]
          });
        }
      } catch (error) {
        Toast({
          msg: 'Could not get error message'
        });
      }
      return Promise.reject(error);
    }
  }
);
export { Axios };

export const formatCurrency = (currency: number) =>
  currency.toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$&,');
