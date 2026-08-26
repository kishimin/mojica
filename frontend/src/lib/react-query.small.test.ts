import { describe, expect, test } from "vitest";
import { queryClient } from "./react-query";

describe("TanStack Query default contract", () => {
  // ID: FOUNDATION-QUERY-S-001
  // Source: docs/v1/ui/implementation-plan.md §1 feat/mojica-ui-foundation
  // Given: a query using the shared TanStack Query configuration has failed
  // When: TanStack Query applies its default failure policy
  // Then: it does not retry the request automatically
  // Priority: P1
  test("does not retry failed queries automatically", async () => {
    let attempts = 0;

    await expect(
      queryClient.fetchQuery({
        queryKey: ["failed-query"],
        queryFn: () => {
          attempts += 1;
          throw new Error("query failed");
        },
      }),
    ).rejects.toThrow("query failed");

    expect(attempts).toBe(1);
    queryClient.clear();
  });

  // ID: FOUNDATION-QUERY-S-002
  // Source: docs/v1/ui/implementation-plan.md §1 feat/mojica-ui-foundation
  // Given: a successful query is cached by the shared QueryClient
  // When: the browser window regains focus
  // Then: it does not refetch solely because focus returned
  // Blocked by: QueryClient wiring that consumes the shared defaults
  // Priority: P1
  test.todo("does not refetch cached queries when window focus returns");
});
