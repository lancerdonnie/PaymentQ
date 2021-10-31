import { toast, TypeOptions } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';

const Toast = ({
  duration = 2000,
  msg = 'Default message',
  type = 'default'
}: {
  duration?: null | number;
  msg?: string;
  type?: TypeOptions;
}) => {
  toast(msg, {
    type,
    position: toast.POSITION.TOP_CENTER,
    autoClose: duration === null ? false : duration,
    hideProgressBar: true
  });
};

export default Toast;
