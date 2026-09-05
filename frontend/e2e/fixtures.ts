import { expect, test as base } from "@playwright/test";
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
  imageGenerationPage: ImageGenerationPage;
  notFoundPage: NotFoundPage;
  errorFallbackPage: ErrorFallbackPage;
}>({
  imageGenerationPage: async ({ page }, use) => {
    // oxlint-disable-next-line react-hooks/rules-of-hooks -- Playwright fixture callbacks expose a required use function.
    await use(imageGenerationPage(page));
  },
  notFoundPage: async ({ page }, use) => {
    // oxlint-disable-next-line react-hooks/rules-of-hooks -- Playwright fixture callbacks expose a required use function.
    await use(notFoundPage(page));
  },
  errorFallbackPage: async ({ page }, use) => {
    // oxlint-disable-next-line react-hooks/rules-of-hooks -- Playwright fixture callbacks expose a required use function.
    await use(errorFallbackPage(page));
  },
});

export { expect };
