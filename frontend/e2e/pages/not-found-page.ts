import type { Page } from "@playwright/test";
import type { Locale } from "../../src/types/i18n.ts";
import { notFoundSelectors } from "../selectors/not-found-selectors.ts";

/** Provides user-facing operations for the not-found page. */
export const notFoundPage = (page: Page, locale: Locale) => {
  const navigate = async (path: string) => {
    await page.goto(path);
  };

  const homeLink = () =>
    page.getByRole("link", { name: notFoundSelectors.homeLink[locale] });

  return {
    navigate,
    homeLink,
  };
};

export type NotFoundPage = ReturnType<typeof notFoundPage>;
