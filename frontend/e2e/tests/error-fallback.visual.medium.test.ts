import { expect, test } from "../fixtures/test.ts";

test.use({ baseURL: "http://localhost:6006" });

test.describe("ErrorFallback Storybook visual regression", () => {
  test("keeps the Japanese error fallback layout stable", async ({
    errorFallbackPage,
  }) => {
    await test.step("Open the Japanese error fallback story", () =>
      errorFallbackPage.openStory("features-error-errorfallback--japanese"));

    await test.step("Verify the Japanese error heading", () =>
      expect(errorFallbackPage.heading()).toBeVisible());
    await test.step("Compare the Japanese error fallback screenshot", () =>
      errorFallbackPage.compareScreenshot("error-fallback.png"));
  });

  test("keeps the English error fallback layout stable", async ({
    errorFallbackPage,
  }) => {
    await test.step("Open the English error fallback story", () =>
      errorFallbackPage.openStory("features-error-errorfallback--english"));

    await test.step("Verify the English error heading", () =>
      expect(errorFallbackPage.heading()).toBeVisible());
    await test.step("Compare the English error fallback screenshot", () =>
      errorFallbackPage.compareScreenshot("error-fallback-en.png"));
  });
});
