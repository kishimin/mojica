import { focusManager, QueryObserver } from "@tanstack/react-query";
import { afterEach, describe, expect, test } from "vitest";
import { queryClient } from "./react-query";

afterEach(() => {
  queryClient.unmount();
  queryClient.clear();
  focusManager.setFocused(undefined);
});

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
  });

  // ID: FOUNDATION-QUERY-S-002
  // Source: docs/v1/ui/implementation-plan.md §1 feat/mojica-ui-foundation
  // Given: a successful query is cached by the shared QueryClient
  // When: the browser window regains focus
  // Then: it does not refetch solely because focus returned
  // Priority: P1
  test("does not refetch cached queries when window focus returns", async () => {
    let requests = 0;
    let stopObserving = () => {};
    const observer = new QueryObserver(queryClient, {
      queryKey: ["cached-query"],
      queryFn: () => {
        requests += 1;
        return Promise.resolve("cached value");
      },
    });

    focusManager.setFocused(false);
    queryClient.mount();

    try {
      await new Promise<void>((resolve) => {
        stopObserving = observer.subscribe((result) => {
          if (result.isSuccess) resolve();
        });
      });

      focusManager.setFocused(true);

      expect(requests).toBe(1);
    } finally {
      stopObserving();
    }
  });
});
