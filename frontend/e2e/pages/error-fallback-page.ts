import type { Page } from "@playwright/test";

/** Provides user-facing operations for the unexpected-error page. */
export const createErrorFallbackPage = (page: Page) => ({
  /** Returns the recovery action shown by the fallback. */
  reloadButton: () => page.getByRole("button"),
});

export type ErrorFallbackPage = ReturnType<typeof createErrorFallbackPage>;
