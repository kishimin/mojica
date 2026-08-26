import { afterEach, describe, expect, test, vi } from "vitest";
import { queryClient } from "./react-query";

afterEach(() => {
  queryClient.clear();
});

describe("TanStack Query default contract", () => {
  // ID: FOUNDATION-QUERY-S-001
  // Source: docs/v1/ui/implementation-plan.md §1 feat/mojica-ui-foundation
  // Given: a query using the shared TanStack Query configuration has failed
  // When: TanStack Query applies its default failure policy
  // Then: it does not retry the request automatically
  // Priority: P1
  test("does not retry failed queries automatically", async () => {
    const queryFn = vi.fn().mockRejectedValue(new Error("query failed"));

    await expect(
      queryClient.fetchQuery({
        queryKey: ["failed-query"],
        queryFn,
      }),
    ).rejects.toThrow("query failed");

    expect(queryFn).toHaveBeenCalledOnce();
  });
});
