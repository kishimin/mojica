import { expect, test } from "../fixtures.js";

test.describe("navigation", () => {
  test("returns from the not-found view to the image-generation home", async ({
    navigationPage,
  }) => {
    await navigationPage.openNotFoundView();
    await expect(navigationPage.homeLink()).toBeVisible();

    await navigationPage.returnHome();

    await expect(navigationPage.homeHeading()).toBeVisible();
  });
});
