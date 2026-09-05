import type { Page } from "@playwright/test";

/** Provides user-facing operations for the unexpected-error page. */
export const errorFallbackPage = (page: Page) => {
  const reloadButton = () => page.getByRole("button");

  return {
    reloadButton,
  };
};

export type ErrorFallbackPage = ReturnType<typeof errorFallbackPage>;
