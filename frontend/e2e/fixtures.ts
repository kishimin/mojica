import { expect, test as base } from "@playwright/test";
import {
  createErrorFallbackPage,
  type ErrorFallbackPage,
} from "./pages/error-fallback-page.ts";
import {
  createImageGenerationPage,
  type ImageGenerationPage,
} from "./pages/image-generation-page.ts";
import {
  createNotFoundPage,
  type NotFoundPage,
} from "./pages/not-found-page.ts";

/** Shared Playwright fixture entry point for E2E tests. */
export const test = base.extend<{
  imageGenerationPage: ImageGenerationPage;
  notFoundPage: NotFoundPage;
  errorFallbackPage: ErrorFallbackPage;
}>({
  imageGenerationPage: async ({ page }, use) => {
    // oxlint-disable-next-line react-hooks/rules-of-hooks -- Playwright fixture callbacks expose a required use function.
    await use(createImageGenerationPage(page));
  },
  notFoundPage: async ({ page }, use) => {
    // oxlint-disable-next-line react-hooks/rules-of-hooks -- Playwright fixture callbacks expose a required use function.
    await use(createNotFoundPage(page));
  },
  errorFallbackPage: async ({ page }, use) => {
    // oxlint-disable-next-line react-hooks/rules-of-hooks -- Playwright fixture callbacks expose a required use function.
    await use(createErrorFallbackPage(page));
  },
});

export { expect };
