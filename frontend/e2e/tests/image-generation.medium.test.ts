import { expect, test } from "../fixtures/test.ts";

test.describe("image generation", () => {
  test("generates an image through the real API", async ({
    imageGenerationPage,
  }) => {
    await imageGenerationPage.navigate();
    await imageGenerationPage.fillText("KA");
    await imageGenerationPage.fillForegroundCharacter("A");
    await imageGenerationPage.fillBackgroundCharacter("B");

    const download = await imageGenerationPage.submit();

    expect(download.suggestedFilename()).toMatch(/\.png$/);
  });

  test("generates an image when submitted with the keyboard", async ({
    imageGenerationPage,
  }) => {
    await imageGenerationPage.navigate();
    await imageGenerationPage.fillText("KA");
    await imageGenerationPage.fillForegroundCharacter("A");
    await imageGenerationPage.fillBackgroundCharacter("B");
    await imageGenerationPage.tabToSubmitButton();
    await expect(imageGenerationPage.submitButton()).toBeFocused();

    const download = await imageGenerationPage.submitWithKeyboard();

    expect(download.suggestedFilename()).toMatch(/\.png$/);
  });
});
