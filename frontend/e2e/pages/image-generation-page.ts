import type { Download, Page } from "@playwright/test";
import type { Locale } from "../../src/types/i18n.ts";
import {
  imageGenerationBackgroundCharacterLabel,
  imageGenerationForegroundCharacterLabel,
  imageGenerationHeadingName,
  imageGenerationSubmitButtonName,
  imageGenerationTextLabel,
} from "../selectors/image-generation-selectors.ts";

/** Provides user-facing image-generation operations for browser tests. */
export const imageGenerationPage = (page: Page, locale: Locale) => {
  const textInput = () =>
    page.getByRole("textbox", { name: imageGenerationTextLabel(locale) });
  const foregroundCharacterInput = () =>
    page.getByRole("textbox", {
      name: imageGenerationForegroundCharacterLabel(locale),
      exact: true,
    });
  const backgroundCharacterInput = () =>
    page.getByRole("textbox", {
      name: imageGenerationBackgroundCharacterLabel(locale),
      exact: true,
    });
  const submitButton = () =>
    page.getByRole("button", { name: imageGenerationSubmitButtonName(locale) });
  const heading = () =>
    page.getByRole("heading", { name: imageGenerationHeadingName(locale) });

  const navigate = async () => {
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
    navigate,
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
