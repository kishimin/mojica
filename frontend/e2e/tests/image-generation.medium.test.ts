import { expect, test } from "../fixtures/test.ts";

test.describe("image generation", () => {
  test("uploads an image and downloads the generated preview", async ({ imageGenerationPage }) => {
    await imageGenerationPage.navigate();
    await imageGenerationPage.fillImageMode({
      name: "frame.png",
      mimeType: "image/png",
      buffer: Buffer.from("iVBORw0KGgoAAAANSUhEUgAAAAoAAAAKCAYAAACNMs+9AAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAASSURBVChTY2CgJfiPLjAKiAYAPG0BAOp2pS8AAAAASUVORK5CYII=", "base64"),
    }, "A", "B");
    const download = await imageGenerationPage.submit();
    expect(download.suggestedFilename()).toMatch(/\.png$/);
  });
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
