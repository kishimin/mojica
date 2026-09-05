import type { Page } from "@playwright/test";
import { notFoundSelectors } from "../selectors/not-found-selectors.ts";

/** Provides user-facing operations for the not-found page. */
export const notFoundPage = (page: Page) => ({
  /** Opens a route that the application does not define. */
  navigate: async () => {
    await page.goto("/missing");
  },

  /** Returns the recovery link to the image-generation home. */
  homeLink: () => page.getByRole("link", { name: notFoundSelectors.homeLink }),
});

export type NotFoundPage = ReturnType<typeof notFoundPage>;
