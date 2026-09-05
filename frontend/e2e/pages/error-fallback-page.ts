import { expect, type Page } from "@playwright/test";
import type { Locale } from "../../src/types/i18n.ts";
import { errorFallbackReloadButtonName } from "../selectors/error-fallback-selectors.ts";

/** Provides user-facing operations for the unexpected-error page. */
export const errorFallbackPage = (page: Page) => {
  const openStory = async (storyId: string) => {
    await page.goto(`/iframe.html?id=${storyId}&viewMode=story`);
  };

  const reloadButton = (locale: Locale) =>
    page.getByRole("button", {
      name: errorFallbackReloadButtonName(locale),
    });
  const heading = () => page.getByRole("heading");

  return {
    openStory,
    heading,
    reloadButton,
    compareScreenshot: async (name: string) => {
      await expect(page).toHaveScreenshot(name, { fullPage: true });
    },
  };
};

export type ErrorFallbackPage = ReturnType<typeof errorFallbackPage>;
