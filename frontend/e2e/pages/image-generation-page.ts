import type { Download, Page } from "@playwright/test";
import { imageGenerationSelectors } from "../selectors/image-generation-selectors.ts";

/** Provides user-facing image-generation operations for browser tests. */
export const imageGenerationPage = (page: Page) => {
  const textInput = () =>
    page.getByRole("textbox", { name: imageGenerationSelectors.text });
  const foregroundCharacterInput = () =>
    page.getByRole("textbox", {
      name: imageGenerationSelectors.foregroundCharacter,
      exact: true,
    });
  const backgroundCharacterInput = () =>
    page.getByRole("textbox", {
      name: imageGenerationSelectors.backgroundCharacter,
      exact: true,
    });
  const submitButton = () =>
    page.getByRole("button", { name: imageGenerationSelectors.submit });
  const heading = () =>
    page.getByRole("heading", { name: imageGenerationSelectors.heading });

  const goto = async () => {
    await page.goto("/");
  };

  const fillText = async (value: string) => {
    await textInput().fill(value);
  };

  const fillForegroundCharacter = async (value: string) => {
    await foregroundCharacterInput().fill(value);
  };

  const fillBackgroundCharacter = async (value: string) => {
    await backgroundCharacterInput().fill(value);
  };

  const submit = async (): Promise<Download> => {
    const downloadPromise = page.waitForEvent("download");
    await submitButton().click();

    return downloadPromise;
  };

  return {
    goto,
    fillText,
    fillForegroundCharacter,
    fillBackgroundCharacter,
    submit,
    heading,
    textInput,
    foregroundCharacterInput,
    backgroundCharacterInput,
    submitButton,
  };
};

export type ImageGenerationPage = ReturnType<typeof imageGenerationPage>;
