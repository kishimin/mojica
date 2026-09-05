import { test as base } from "@playwright/test";
import type { Locale } from "../../src/types/i18n.ts";
import {
  errorFallbackPage,
  type ErrorFallbackPage,
} from "../pages/error-fallback-page.ts";
import {
  imageGenerationPage,
  type ImageGenerationPage,
} from "../pages/image-generation-page.ts";
import { notFoundPage, type NotFoundPage } from "../pages/not-found-page.ts";

/** Shared Playwright fixture entry point for E2E tests. */
type E2EFixtures = {
  locale: Locale;
  imageGenerationPage: ImageGenerationPage;
  notFoundPage: NotFoundPage;
  errorFallbackPage: ErrorFallbackPage;
};

export const test = base.extend<E2EFixtures>({
  locale: async ({ browserName: _browserName }, provide) => {
    void _browserName;
    await provide("ja");
  },
  imageGenerationPage: async ({ page, locale }, provide) => {
    await page.addInitScript((selectedLocale) => {
      localStorage.setItem("locale", selectedLocale);
    }, locale);
    await provide(imageGenerationPage(page, locale));
  },
  notFoundPage: async ({ page, locale }, provide) => {
    await page.addInitScript((selectedLocale) => {
      localStorage.setItem("locale", selectedLocale);
    }, locale);
    await provide(notFoundPage(page, locale));
  },
  errorFallbackPage: async ({ page }, provide) => {
    await provide(errorFallbackPage(page));
  },
});

export { expect } from "@playwright/test";
