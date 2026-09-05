import { expect, type Locator, type Page } from "@playwright/test";

/** Provides navigation and screenshot operations for visual regression tests. */
export const visualRegressionPage = (page: Page) => {
  const imageGenerationHeading = (): Locator =>
    page.getByRole("heading", { name: /文字で、文字を描く。/ });
  const notFoundHeading = (): Locator =>
    page.getByRole("heading", { name: "404" });
  const errorFallbackHeading = (): Locator =>
    page.getByRole("heading", {
      name: /エラーが発生しました|An error occurred/,
    });
  const openHome = async () => page.goto("/");
  const openNotFound = async () => page.goto("/missing");
  const openErrorFallbackStory = async () =>
    page.goto(
      "http://localhost:6006/iframe.html?id=features-error-errorfallback--default",
    );
  const compareScreenshot = async (name: string) =>
    expect(page).toHaveScreenshot(name, { fullPage: true });

  return {
    imageGenerationHeading,
    notFoundHeading,
    errorFallbackHeading,
    openHome,
    openNotFound,
    openErrorFallbackStory,
    compareScreenshot,
  };
};

export type VisualRegressionPage = ReturnType<typeof visualRegressionPage>;
