import type { Page } from "@playwright/test";

/** Provides user-facing operations for the not-found page. */
export const notFoundPage = (page: Page) => ({
  /** Opens a route that the application does not define. */
  open: async () => {
    await page.goto("/missing");
  },

  /** Returns the recovery link to the image-generation home. */
  homeLink: () => page.getByRole("link", { name: "トップページへ戻る" }),
});

export type NotFoundPage = ReturnType<typeof notFoundPage>;
