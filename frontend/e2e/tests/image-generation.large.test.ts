import { imageTypeDefinitions } from "../../src/types/image-type.ts";
import { expect, test } from "../fixtures/test.ts";

const releaseBaseUrl = process.env.RELEASE_E2E_BASE_URL;
const imageTypes = Object.values(imageTypeDefinitions);

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

for (const locale of ["ja", "en"] as const) {
  test.describe(
    locale === "ja" ? "Japanese release flow" : "English release flow",
    () => {
      test.use({ appLocale: locale });

      for (const imageType of imageTypes) {
        test(`generates and downloads ${imageType} through the deployed API`, async ({
          imageGenerationPage,
        }, testInfo) => {
          await imageGenerationPage.navigate();
          await imageGenerationPage.fillText("KA");
          await imageGenerationPage.fillForegroundCharacter("A");
          await imageGenerationPage.fillBackgroundCharacter("B");
          await imageGenerationPage.selectType(imageType);

          const download = await imageGenerationPage.submit();

          expect(download.suggestedFilename()).toMatch(/\.png$/);
          await imageGenerationPage.captureScreenshot(
            testInfo.outputPath(`${locale}-${imageType}.png`),
          );
        });
      }
    },
  );
}
