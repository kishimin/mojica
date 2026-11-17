import { expect, type Download, type Page } from "@playwright/test";
import type { Locale } from "../../src/types/i18n.ts";
import type { ImageType } from "../../src/types/image-type.ts";
import {
  imageGenerationSelectors,
  imageTypeOptionSelectors,
} from "../selectors/image-generation-selectors.ts";

/** Provides user-facing image-generation operations for browser tests. */
export const imageGenerationPage = (page: Page, locale: Locale) => {
  const textInput = () =>
    page.getByRole("textbox", {
      name: imageGenerationSelectors.textLabel[locale],
    });
  const foregroundCharacterInput = () =>
    page.getByRole("textbox", {
      name: imageGenerationSelectors.foregroundCharacterLabel[locale],
    });
  const backgroundCharacterInput = () =>
    page.getByRole("textbox", {
      name: imageGenerationSelectors.backgroundCharacterLabel[locale],
    });
  const foregroundColorInput = () =>
    page.getByRole("textbox", {
      name: imageGenerationSelectors.foregroundColorLabel[locale],
    });
  const backgroundColorInput = () =>
    page.getByRole("textbox", {
      name: imageGenerationSelectors.backgroundColorLabel[locale],
    });
  const submitButton = () =>
    page.getByRole("button", {
      name: imageGenerationSelectors.submitButton[locale],
    });
  const imageTypeSelect = () =>
    page.getByRole("combobox", {
      name: imageGenerationSelectors.imageTypeLabel[locale],
    });
  const heading = () =>
    page.getByRole("heading", {
      name: imageGenerationSelectors.heading[locale],
    });

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

  const fillForegroundColor = async (value: string) => {
    await foregroundColorInput().fill(value);
  };

  const fillBackgroundColor = async (value: string) => {
    await backgroundColorInput().fill(value);
  };

  const selectType = async (value: ImageType) => {
    await imageTypeSelect().click();
    await page
      .getByRole("option", {
        name: imageTypeOptionSelectors[value][locale],
      })
      .click();
  };

  const fillImageMode = async (file: { name: string; mimeType: string; buffer: Buffer }, foreground: string, background: string) => {
    await page.getByRole("radio", { name: imageGenerationSelectors.imageMode[locale] }).check();
    await page.getByLabel(imageGenerationSelectors.sourceImage[locale]).setInputFiles(file);
    await page.getByRole("textbox", { name: imageGenerationSelectors.patternCharacters[locale] }).fill(foreground);
    await page.getByRole("textbox", { name: imageGenerationSelectors.imageBackgroundCharacters[locale] }).fill(background);
  };

  const captureScreenshot = async (path: string) => {
    await page.screenshot({ path, fullPage: true });
  };

  const submit = async (): Promise<Download> => {
    await submitButton().click();
    await expect(page.getByRole("img", { name: imageGenerationSelectors.preview[locale] })).toBeVisible();
    const downloadPromise = page.waitForEvent("download");
    await page.getByRole("button", { name: imageGenerationSelectors.downloadButton[locale] }).click();
    return downloadPromise;
  };

  const submitWithKeyboard = async (): Promise<Download> => {
    await submitButton().press("Enter");
    await expect(page.getByRole("img", { name: imageGenerationSelectors.preview[locale] })).toBeVisible();
    const downloadPromise = page.waitForEvent("download");
    await page.getByRole("button", { name: imageGenerationSelectors.downloadButton[locale] }).press("Enter");
    return downloadPromise;
  };

  const compareScreenshot = async (name: string) =>
    expect(page).toHaveScreenshot(name, { fullPage: true });

  return {
    navigate,
    fillText,
    fillForegroundCharacter,
    fillBackgroundCharacter,
    fillForegroundColor,
    fillBackgroundColor,
    selectType,
    fillImageMode,
    submit,
    submitWithKeyboard,
    heading,
    textInput,
    foregroundCharacterInput,
    backgroundCharacterInput,
    foregroundColorInput,
    backgroundColorInput,
    submitButton,
    compareScreenshot,
    captureScreenshot,
  };
};

export type ImageGenerationPage = ReturnType<typeof imageGenerationPage>;
