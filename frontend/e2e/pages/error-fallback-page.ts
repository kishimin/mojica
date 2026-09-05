import type { Page } from "@playwright/test";
import type { Locale } from "../../src/types/i18n.ts";
import { errorFallbackReloadButtonName } from "../selectors/error-fallback-selectors.ts";

/** Provides user-facing operations for the unexpected-error page. */
export const errorFallbackPage = (page: Page) => {
  const reloadButton = (locale: Locale) =>
    page.getByRole("button", {
      name: errorFallbackReloadButtonName(locale),
    });

  return {
    reloadButton,
  };
};

export type ErrorFallbackPage = ReturnType<typeof errorFallbackPage>;
