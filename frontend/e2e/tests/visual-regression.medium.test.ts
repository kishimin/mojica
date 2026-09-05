import { expect, test } from "../fixtures/test.ts";

test.describe("visual regression", () => {
  test("keeps the image-generation home layout stable", async ({
    imageGenerationPage,
  }) => {
    await imageGenerationPage.navigate();

    await expect(imageGenerationPage.heading()).toBeVisible();
    await imageGenerationPage.compareScreenshot("image-generation-home.png");
  });

  test("keeps the not-found layout stable", async ({ notFoundPage }) => {
    await notFoundPage.navigate("/missing");

    await expect(notFoundPage.homeLink()).toBeVisible();
    await notFoundPage.compareScreenshot("not-found.png");
  });

  test.describe("English", () => {
    test.use({ appLocale: "en" });

    test("keeps the English image-generation home layout stable", async ({
      imageGenerationPage,
    }) => {
      await imageGenerationPage.navigate();

      await expect(imageGenerationPage.heading()).toBeVisible();
      await imageGenerationPage.compareScreenshot("image-generation-home-en.png");
    });

    test("keeps the English not-found layout stable", async ({
      notFoundPage,
    }) => {
      await notFoundPage.navigate("/missing");

      await expect(notFoundPage.homeLink()).toBeVisible();
      await notFoundPage.compareScreenshot("not-found-en.png");
    });
  });
});
