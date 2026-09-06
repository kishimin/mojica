import { expect, test } from "../fixtures/test.ts";

test.describe("navigation", () => {
  test("returns from the not-found view to the image-generation home", async ({
    notFoundPage,
    imageGenerationPage,
  }) => {
    await test.step("Open the not-found route", () =>
      notFoundPage.navigate("/missing"));
    await test.step("Verify the home link is available", () =>
      expect(notFoundPage.homeLink()).toBeVisible());

    await test.step("Navigate to the image-generation home", () =>
      notFoundPage.homeLink().click());

    await test.step("Verify the image-generation screen is displayed", () =>
      expect(imageGenerationPage.heading()).toBeVisible());
  });
});
