import { create } from "axios";
import type { AxiosError, AxiosRequestConfig, AxiosResponse } from "axios";

export const AXIOS_INSTANCE = create({
  baseURL: import.meta.env.VITE_API_URL,
});

export const customInstance = <T>(
  config: AxiosRequestConfig,
): Promise<AxiosResponse<T>> => AXIOS_INSTANCE<T>({ ...config });

export default customInstance;

export type ErrorType<Error> = AxiosError<Error>;
