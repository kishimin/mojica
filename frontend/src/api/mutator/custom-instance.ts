import { create } from "axios";
import type { AxiosError, AxiosRequestConfig } from "axios";

export const AXIOS_INSTANCE = create({
  baseURL: import.meta.env.VITE_API_URL,
});

export const customInstance = <T>(config: AxiosRequestConfig): Promise<T> => {
  const promise = AXIOS_INSTANCE({ ...config }).then(
    ({ data }) => data as Promise<T>,
  );

  return promise;
};

export default customInstance;

export type ErrorType<Error> = AxiosError<Error>;
