import { imageTypeDefinitions } from "../../src/types/image-type.ts";
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
  imageType: keyof typeof imageTypeDefinitions,
  screenshotPath: string,
): Promise<void> => {
  await imageGenerationPage.navigate();
  await imageGenerationPage.fillText("KA");
  await imageGenerationPage.fillForegroundCharacter("A");
  await imageGenerationPage.fillBackgroundCharacter("B");
  await imageGenerationPage.selectType(imageTypeDefinitions[imageType]);

  const download = await imageGenerationPage.submit();

  expect(download.suggestedFilename()).toMatch(/\.png$/);
  await imageGenerationPage.captureScreenshot(screenshotPath);
};

test.describe("Japanese release flow", () => {
  test.use({ appLocale: "ja" });

  test("generates and downloads a standard image through the deployed API", async ({
    imageGenerationPage,
  }, testInfo) => {
    await generateAndCapture(
      imageGenerationPage,
      "standard",
      testInfo.outputPath("ja-standard.png"),
    );
  });

  test("generates and downloads an X-background image through the deployed API", async ({
    imageGenerationPage,
  }, testInfo) => {
    await generateAndCapture(
      imageGenerationPage,
      "xBackground",
      testInfo.outputPath("ja-x-background.png"),
    );
  });

  test("generates and downloads an X-icon image through the deployed API", async ({
    imageGenerationPage,
  }, testInfo) => {
    await generateAndCapture(
      imageGenerationPage,
      "xIcon",
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
      "standard",
      testInfo.outputPath("en-standard.png"),
    );
  });

  test("generates and downloads an X-background image through the deployed API", async ({
    imageGenerationPage,
  }, testInfo) => {
    await generateAndCapture(
      imageGenerationPage,
      "xBackground",
      testInfo.outputPath("en-x-background.png"),
    );
  });

  test("generates and downloads an X-icon image through the deployed API", async ({
    imageGenerationPage,
  }, testInfo) => {
    await generateAndCapture(
      imageGenerationPage,
      "xIcon",
      testInfo.outputPath("en-x-icon.png"),
    );
  });
});
