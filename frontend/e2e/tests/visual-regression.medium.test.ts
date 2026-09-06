import { expect, test } from "../fixtures/test.ts";

test.describe("visual regression", () => {
  test("keeps the image-generation home layout stable", async ({
    imageGenerationPage,
  }) => {
    await test.step("Open the image-generation home", () =>
      imageGenerationPage.navigate());

    await test.step("Verify the image-generation heading", () =>
      expect(imageGenerationPage.heading()).toBeVisible());
    await test.step("Compare the image-generation screenshot", () =>
      imageGenerationPage.compareScreenshot("image-generation-home.png"));
  });

  test("keeps the not-found layout stable", async ({ notFoundPage }) => {
    await test.step("Open the not-found route", () =>
      notFoundPage.navigate("/missing"));

    await test.step("Verify the home link", () =>
      expect(notFoundPage.homeLink()).toBeVisible());
    await test.step("Compare the not-found screenshot", () =>
      notFoundPage.compareScreenshot("not-found.png"));
  });

  test.describe("English", () => {
    test.use({ appLocale: "en" });

    test("keeps the English image-generation home layout stable", async ({
      imageGenerationPage,
    }) => {
      await test.step("Open the English image-generation home", () =>
        imageGenerationPage.navigate());

      await test.step("Verify the English image-generation heading", () =>
        expect(imageGenerationPage.heading()).toBeVisible());
      await test.step("Compare the English image-generation screenshot", () =>
        imageGenerationPage.compareScreenshot("image-generation-home-en.png"));
    });

    test("keeps the English not-found layout stable", async ({
      notFoundPage,
    }) => {
      await test.step("Open the English not-found route", () =>
        notFoundPage.navigate("/missing"));

      await test.step("Verify the English home link", () =>
        expect(notFoundPage.homeLink()).toBeVisible());
      await test.step("Compare the English not-found screenshot", () =>
        notFoundPage.compareScreenshot("not-found-en.png"));
    });
  });
});
