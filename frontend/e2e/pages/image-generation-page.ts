import type { Download, Page } from "@playwright/test";
import { imageGenerationSelectors } from "../selectors/image-generation-selectors.ts";

/** Provides user-facing image-generation operations for browser tests. */
export const imageGenerationPage = (page: Page) => {
  const goto = async () => {
    await page.goto("/");
  };

  const fillText = async (value: string) => {
    await page
      .getByRole("textbox", { name: imageGenerationSelectors.text })
      .fill(value);
  };

  const fillForegroundCharacter = async (value: string) => {
    await page
      .getByRole("textbox", {
        name: imageGenerationSelectors.foregroundCharacter,
        exact: true,
      })
      .fill(value);
  };

  const fillBackgroundCharacter = async (value: string) => {
    await page
      .getByRole("textbox", {
        name: imageGenerationSelectors.backgroundCharacter,
        exact: true,
      })
      .fill(value);
  };

  const submit = async (): Promise<Download> => {
    const downloadPromise = page.waitForEvent("download");
    await page
      .getByRole("button", { name: imageGenerationSelectors.submit })
      .click();

    return downloadPromise;
  };

  const heading = () =>
    page.getByRole("heading", { name: imageGenerationSelectors.heading });

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
