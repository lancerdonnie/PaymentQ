import { Axios } from 'Utils';

const apiMutate = async (
  url: string,
  postData: any,
  method?: 'put' | 'delete'
) =>
  (
    await Axios({
      method: method || 'post',
      url,
      data: postData
    })
  ).data;

export default apiMutate;
