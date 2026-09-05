import type { Download, Page } from "@playwright/test";

/** Provides user-facing image-generation operations for browser tests. */
export const createImageGenerationPage = (page: Page) => ({
  /** Returns the image-generation page heading. */
  heading: () => page.getByRole("heading", { name: "文字で、文字を描く。" }),

  /** Submits the documented valid request and returns the generated download. */
  generateImage: async (): Promise<Download> => {
    await page.goto("/");
    await page.getByRole("textbox", { name: "描画する文字列" }).fill("KA");
    await page
      .getByRole("textbox", { name: "描画に使う文字", exact: true })
      .fill("A");
    await page
      .getByRole("textbox", { name: "敷き詰める文字", exact: true })
      .fill("B");

    const downloadPromise = page.waitForEvent("download");
    await page.getByRole("button", { name: "画像を生成する" }).click();

    return downloadPromise;
  },
});

export type ImageGenerationPage = ReturnType<typeof createImageGenerationPage>;
