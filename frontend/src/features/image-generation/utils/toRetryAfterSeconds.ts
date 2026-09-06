import type { AxiosResponse } from "axios";

type RetryAfterHeaderSource = {
  [key: string]: unknown;
};

/** Provides the module's public behavior. */
export const toRetryAfterSeconds = (
  headers: AxiosResponse<unknown>["headers"] | undefined,
) => {
  const retryAfterHeader =
    headers && typeof headers === "object"
      ? (headers as RetryAfterHeaderSource)["retry-after"]
      : undefined;
  const retryAfter =
    typeof retryAfterHeader === "string"
      ? Number.parseInt(retryAfterHeader, 10)
      : 0;

  return Number.isFinite(retryAfter) ? retryAfter : 0;
};
