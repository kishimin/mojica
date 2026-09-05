import { expect, test } from "../fixtures.ts";

test.describe("navigation", () => {
  test("returns from the not-found view to the image-generation home", async ({
    notFoundPage,
    imageGenerationPage,
  }) => {
    await notFoundPage.navigate("/missing");
    await expect(notFoundPage.homeLink()).toBeVisible();

    await notFoundPage.homeLink().click();

    await expect(imageGenerationPage.heading()).toBeVisible();
  });
});
