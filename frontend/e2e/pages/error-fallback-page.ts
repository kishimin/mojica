import type { Page } from "@playwright/test";
import { errorFallbackSelectors } from "../selectors/error-fallback-selectors.ts";

/** Provides user-facing operations for the unexpected-error page. */
export const errorFallbackPage = (page: Page) => {
  const reloadButton = () =>
    page.getByRole("button", { name: errorFallbackSelectors.reloadButton });

  return {
    reloadButton,
  };
};

export type ErrorFallbackPage = ReturnType<typeof errorFallbackPage>;
