import {
  imageTypeDefinitions,
  type ImageType,
} from "../../src/types/image-type.ts";
import { expect, test } from "../fixtures/test.ts";
import type { ImageGenerationPage } from "../pages/image-generation-page.ts";

const releaseBaseUrl = process.env.RELEASE_E2E_BASE_URL;
test.skip(
  !releaseBaseUrl,
  "Set RELEASE_E2E_BASE_URL to run against the deployed service.",
);
test.use({
  baseURL: releaseBaseUrl,
  screenshot: "on",
  video: "on",
});

test.describe.configure({ mode: "serial" });

const generateAndCapture = async (
  imageGenerationPage: ImageGenerationPage,
  imageType: ImageType,
  downloadPath: string,
  screenshotPath: string,
): Promise<void> => {
  await test.step("Open the image-generation screen", () =>
    imageGenerationPage.navigate());
  await test.step("Fill the image-generation inputs", async () => {
    await imageGenerationPage.fillText("KA");
    await imageGenerationPage.fillForegroundCharacter("A");
    await imageGenerationPage.fillBackgroundCharacter("B");
    await imageGenerationPage.selectType(imageType);
  });

  const download = await test.step("Submit the image-generation request", () =>
    imageGenerationPage.submit());

  await test.step("Save and verify the PNG download", async () => {
    expect(download.suggestedFilename()).toMatch(
      new RegExp(`^mojica-${imageType}-[0-9a-f-]+\\.png$`),
    );
    await download.saveAs(downloadPath);
  });

  await test.step("Capture the screen", async () => {
    await imageGenerationPage.captureScreenshot(screenshotPath);
  });
};

test.describe("Japanese release flow", () => {
  test.use({ appLocale: "ja" });

  test("generates and downloads a standard image through the deployed API", async ({
    imageGenerationPage,
  }, testInfo) => {
    await generateAndCapture(
      imageGenerationPage,
      imageTypeDefinitions.standard,
      testInfo.outputPath("ja-standard-download.png"),
      testInfo.outputPath("ja-standard.png"),
    );
  });

  test("generates and downloads an X-background image through the deployed API", async ({
    imageGenerationPage,
  }, testInfo) => {
    await generateAndCapture(
      imageGenerationPage,
      imageTypeDefinitions.xBackground,
      testInfo.outputPath("ja-x-background-download.png"),
      testInfo.outputPath("ja-x-background.png"),
    );
  });

  test("generates and downloads an X-icon image through the deployed API", async ({
    imageGenerationPage,
  }, testInfo) => {
    await generateAndCapture(
      imageGenerationPage,
      imageTypeDefinitions.xIcon,
      testInfo.outputPath("ja-x-icon-download.png"),
      testInfo.outputPath("ja-x-icon.png"),
    );
  });
});

test.describe("English release flow", () => {
  test.use({ appLocale: "en" });

  test("generates and downloads a standard image through the deployed API", async ({
    imageGenerationPage,
  }, testInfo) => {
    await generateAndCapture(
      imageGenerationPage,
      imageTypeDefinitions.standard,
      testInfo.outputPath("en-standard-download.png"),
      testInfo.outputPath("en-standard.png"),
    );
  });

  test("generates and downloads an X-background image through the deployed API", async ({
    imageGenerationPage,
  }, testInfo) => {
    await generateAndCapture(
      imageGenerationPage,
      imageTypeDefinitions.xBackground,
      testInfo.outputPath("en-x-background-download.png"),
      testInfo.outputPath("en-x-background.png"),
    );
  });

  test("generates and downloads an X-icon image through the deployed API", async ({
    imageGenerationPage,
  }, testInfo) => {
    await generateAndCapture(
      imageGenerationPage,
      imageTypeDefinitions.xIcon,
      testInfo.outputPath("en-x-icon-download.png"),
      testInfo.outputPath("en-x-icon.png"),
    );
  });
});
