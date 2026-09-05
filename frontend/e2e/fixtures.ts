import { expect, test as base } from "@playwright/test";
import {
  createNavigationPage,
  type NavigationPage,
} from "./pages/navigation-page.js";
import {
  createImageGenerationPage,
  type ImageGenerationPage,
} from "./pages/image-generation-page.js";

/** Shared Playwright fixture entry point for E2E tests. */
export const test = base.extend<{
  navigationPage: NavigationPage;
  imageGenerationPage: ImageGenerationPage;
}>({
  navigationPage: async ({ page }, use) => {
    await use(createNavigationPage(page));
  },
  imageGenerationPage: async ({ page }, use) => {
    await use(createImageGenerationPage(page));
  },
});

export { expect };
