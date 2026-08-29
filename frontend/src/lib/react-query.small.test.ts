import { afterEach, describe, expect, test, vi } from "vitest";
import { queryClient } from "./react-query";

afterEach(() => {
  queryClient.clear();
});

describe("TanStack Query default contract", () => {
  test("does not retry failed queries automatically", async () => {
    const queryFn = vi
      .fn<() => Promise<never>>()
      .mockRejectedValue(new Error("query failed"));

    await expect(
      queryClient.fetchQuery({
        queryKey: ["failed-query"],
        queryFn,
      }),
    ).rejects.toThrow("query failed");

    expect(queryFn).toHaveBeenCalledOnce();
  });
});
