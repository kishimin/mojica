import { expect, test } from "@playwright/test";

test.describe("image generation", () => {
  test("downloads the generated PNG after submitting valid input", async ({
    page,
  }) => {
    await page.route("**/images", async (route) => {
      await route.fulfill({
        status: 200,
        contentType: "image/png",
        headers: {
          "Content-Disposition": 'attachment; filename="generated-image.png"',
        },
        body: Buffer.from([137, 80, 78, 71]),
      });
    });

    await page.goto("/");
    await page
      .getByRole("textbox", { name: "描画する文字列", exact: true })
      .fill("KA");
    await page
      .getByRole("textbox", { name: "描画に使う文字", exact: true })
      .fill("A");
    await page
      .getByRole("textbox", { name: "敷き詰める文字", exact: true })
      .fill("☀");

    const downloadPromise = page.waitForEvent("download");
    await page.getByRole("button", { name: "画像を生成する" }).click();

    const download = await downloadPromise;
    await expect(download.suggestedFilename()).toBe("generated-image.png");
  });
});
