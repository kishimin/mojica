import { expect, test } from "../fixtures/test.ts";

test.describe("visual regression", () => {
  test("keeps the image-generation home layout stable", async ({
    visualRegressionPage,
  }) => {
    await visualRegressionPage.openHome();

    await expect(visualRegressionPage.imageGenerationHeading()).toBeVisible();
    await visualRegressionPage.compareScreenshot("image-generation-home.png");
  });

  test("keeps the not-found layout stable", async ({
    visualRegressionPage,
  }) => {
    await visualRegressionPage.openNotFound();

    await expect(visualRegressionPage.notFoundHeading()).toBeVisible();
    await visualRegressionPage.compareScreenshot("not-found.png");
  });
});
