import { expect, test } from "../fixtures/test.ts";

test.use({ baseURL: "http://localhost:6006" });

test.describe("ErrorFallback Storybook visual regression", () => {
  test("keeps the Japanese error fallback layout stable", async ({
    errorFallbackPage,
  }) => {
    await errorFallbackPage.openStory("features-error-errorfallback--default");

    await expect(errorFallbackPage.heading()).toBeVisible();
    await errorFallbackPage.compareScreenshot("error-fallback.png");
  });

  test("keeps the English error fallback layout stable", async ({
    errorFallbackPage,
  }) => {
    await errorFallbackPage.openStory("features-error-errorfallback--english");

    await expect(errorFallbackPage.heading()).toBeVisible();
    await errorFallbackPage.compareScreenshot("error-fallback-en.png");
  });
});
