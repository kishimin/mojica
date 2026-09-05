import type { Page } from "@playwright/test";
import { notFoundSelectors } from "../selectors/not-found-selectors.ts";

/** Provides user-facing operations for the not-found page. */
export const notFoundPage = (page: Page) => {
  const navigate = async () => {
    await page.goto("/missing");
  };

  const homeLink = () =>
    page.getByRole("link", { name: notFoundSelectors.homeLink });

  return {
    navigate,
    homeLink,
  };
};

export type NotFoundPage = ReturnType<typeof notFoundPage>;
