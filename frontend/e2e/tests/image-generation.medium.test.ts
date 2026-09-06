import { expect, test } from "../fixtures/test.ts";

test.describe("image generation", () => {
  test("generates an image through the real API", async ({
    imageGenerationPage,
  }) => {
    await test.step("Open the image-generation screen", () =>
      imageGenerationPage.navigate());
    await test.step("Fill the image-generation inputs", async () => {
      await imageGenerationPage.fillText("KA");
      await imageGenerationPage.fillForegroundCharacter("A");
      await imageGenerationPage.fillBackgroundCharacter("B");
    });

    const download =
      await test.step("Submit the image-generation request", () =>
        imageGenerationPage.submit());

    await test.step("Verify the PNG download", () => {
      expect(download.suggestedFilename()).toMatch(/\.png$/);
    });
  });

  test("generates an image when submitted with the keyboard", async ({
    imageGenerationPage,
  }) => {
    await test.step("Open the image-generation screen", () =>
      imageGenerationPage.navigate());
    await test.step("Fill the image-generation inputs", async () => {
      await imageGenerationPage.fillText("KA");
      await imageGenerationPage.fillForegroundCharacter("A");
      await imageGenerationPage.fillBackgroundCharacter("B");
    });

    const download =
      await test.step("Submit the image-generation request with the keyboard", () =>
        imageGenerationPage.submitWithKeyboard());

    await test.step("Verify the PNG download", () => {
      expect(download.suggestedFilename()).toMatch(/\.png$/);
    });
  });
});
