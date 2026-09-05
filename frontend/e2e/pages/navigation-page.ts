import type { Page } from "@playwright/test";

/** Provides user-facing navigation operations for the application shell. */
export const createNavigationPage = (page: Page) => ({
  /** Opens a route that the application does not define. */
  openNotFoundView: async () => {
    await page.goto("/missing");
  },

  /** Returns the recovery link from the not-found view. */
  homeLink: () => page.getByRole("link", { name: "トップページへ戻る" }),

  /** Follows the not-found recovery link to the application home. */
  returnHome: async () => {
    await page.getByRole("link", { name: "トップページへ戻る" }).click();
  },

  /** Returns the home page heading. */
  homeHeading: () =>
    page.getByRole("heading", { name: "文字で、文字を描く。" }),
});

export type NavigationPage = ReturnType<typeof createNavigationPage>;
