import { expect, test } from "../fixtures/test.ts";

test("keeps the ErrorFallback story layout stable", async ({
  visualRegressionPage,
}) => {
  await visualRegressionPage.openErrorFallbackStory();

  await expect(visualRegressionPage.errorFallbackHeading()).toBeVisible();
  await visualRegressionPage.compareScreenshot("error-fallback-story.png");
});
