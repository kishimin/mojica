import { expect, type Locator, type Page } from "@playwright/test";
import { visualRegressionSelectors } from "../selectors/visual-regression-selectors.ts";

/** Provides navigation and screenshot operations for visual regression tests. */
export const visualRegressionPage = (page: Page) => {
  const imageGenerationHeading = (): Locator =>
    page.getByRole("heading", {
      name: visualRegressionSelectors.imageGenerationHeading,
    });
  const notFoundHeading = (): Locator =>
    page.getByRole("heading", {
      name: visualRegressionSelectors.notFoundHeading,
    });
  const openHome = async () => page.goto("/");
  const openNotFound = async () => page.goto("/missing");
  const compareScreenshot = async (name: string) =>
    expect(page).toHaveScreenshot(name, { fullPage: true });

  return {
    imageGenerationHeading,
    notFoundHeading,
    openHome,
    openNotFound,
    compareScreenshot,
  };
};

export type VisualRegressionPage = ReturnType<typeof visualRegressionPage>;
