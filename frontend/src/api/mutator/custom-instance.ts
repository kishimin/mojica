import { create } from "axios";
import type { AxiosError, AxiosRequestConfig, AxiosResponse } from "axios";

export const AXIOS_INSTANCE = create({
  baseURL: import.meta.env.VITE_API_URL,
});

/** Provides the module's public behavior. */
export const customInstance = <T>(
  config: AxiosRequestConfig,
): Promise<AxiosResponse<T>> => {
  const locale = localStorage.getItem("locale") ?? "ja";

  return AXIOS_INSTANCE<T>({
    ...config,
    headers: {
      ...config.headers,
      "Accept-Language": locale,
    },
  });
};

export default customInstance;

export type ErrorType<Error> = AxiosError<Error>;
