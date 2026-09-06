import { imageTypeDefinitions } from "../../src/types/image-type.ts";
import { expect, test } from "../fixtures/test.ts";

const releaseBaseUrl = process.env.RELEASE_E2E_BASE_URL;
const imageTypes = Object.values(imageTypeDefinitions);
const releaseLocales = [
  { locale: "ja", name: "Japanese release flow" },
  { locale: "en", name: "English release flow" },
] as const;

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

for (const { locale, name } of releaseLocales) {
  test.describe(name, () => {
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
  });
}
