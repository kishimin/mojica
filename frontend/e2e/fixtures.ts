import { expect, test as base } from "@playwright/test";
import type { Locale } from "../src/types/i18n.ts";
import {
  errorFallbackPage,
  type ErrorFallbackPage,
} from "./pages/error-fallback-page.ts";
import {
  imageGenerationPage,
  type ImageGenerationPage,
} from "./pages/image-generation-page.ts";
import { notFoundPage, type NotFoundPage } from "./pages/not-found-page.ts";

/** Shared Playwright fixture entry point for E2E tests. */
export const test = base.extend<{
  locale: Locale;
  imageGenerationPage: ImageGenerationPage;
  notFoundPage: NotFoundPage;
  errorFallbackPage: ErrorFallbackPage;
}>({
  locale: async (_fixtures, use) => {
    // oxlint-disable-next-line react-hooks/rules-of-hooks -- Playwright fixture callbacks expose a required use function.
    await use("ja");
  },
  imageGenerationPage: async ({ page, locale }, use) => {
    await page.addInitScript((selectedLocale) => {
      localStorage.setItem("locale", selectedLocale);
    }, locale);
    // oxlint-disable-next-line react-hooks/rules-of-hooks -- Playwright fixture callbacks expose a required use function.
    await use(imageGenerationPage(page, locale));
  },
  notFoundPage: async ({ page, locale }, use) => {
    await page.addInitScript((selectedLocale) => {
      localStorage.setItem("locale", selectedLocale);
    }, locale);
    // oxlint-disable-next-line react-hooks/rules-of-hooks -- Playwright fixture callbacks expose a required use function.
    await use(notFoundPage(page, locale));
  },
  errorFallbackPage: async ({ page }, use) => {
    // oxlint-disable-next-line react-hooks/rules-of-hooks -- Playwright fixture callbacks expose a required use function.
    await use(errorFallbackPage(page));
  },
});

export { expect };
