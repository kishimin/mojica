import type { Download, Page } from "@playwright/test";

/** Provides user-facing image-generation operations for browser tests. */
export const imageGenerationPage = (page: Page) => {
  const goto = async () => {
    await page.goto("/");
  };

  const fillText = async (value: string) => {
    await page.getByRole("textbox", { name: "描画する文字列" }).fill(value);
  };

  const fillForegroundCharacter = async (value: string) => {
    await page
      .getByRole("textbox", { name: "描画に使う文字", exact: true })
      .fill(value);
  };

  const fillBackgroundCharacter = async (value: string) => {
    await page
      .getByRole("textbox", { name: "敷き詰める文字", exact: true })
      .fill(value);
  };

  const submit = async (): Promise<Download> => {
    const downloadPromise = page.waitForEvent("download");
    await page.getByRole("button", { name: "画像を生成する" }).click();

    return downloadPromise;
  };

  const heading = () =>
    page.getByRole("heading", { name: "文字で、文字を描く。" });

  return {
    goto,
    fillText,
    fillForegroundCharacter,
    fillBackgroundCharacter,
    submit,
    heading,
  };
};

export type ImageGenerationPage = ReturnType<typeof imageGenerationPage>;
