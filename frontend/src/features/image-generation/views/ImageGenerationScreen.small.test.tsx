import { screen } from "@testing-library/react";
import { describe, expect, test } from "vitest";
import ImageGenerationScreen from "./ImageGenerationScreen";
import { setupWithProviders } from "@/tests/test-utils";
import type { Locale } from "@/types/i18n";

const renderScreen = (locale: Locale) =>
  setupWithProviders(<ImageGenerationScreen />, locale);

describe("ImageGenerationScreen", () => {
  describe("Japanese copy", () => {
    test("renders the Japanese image-generation heading", () => {
      renderScreen("ja");

      expect(
        screen.getByRole("heading", { name: "文字で、文字を描く。" }),
      ).toBeVisible();
    });

    test("renders the Japanese image-generation description", () => {
      renderScreen("ja");

      expect(
        screen.getByText(
          "好きな文字と2つの色を組み合わせて、文字アート画像を生成します。",
        ),
      ).toBeVisible();
    });
  });

  describe("English copy", () => {
    test("renders the English image-generation heading", () => {
      renderScreen("en");

      expect(
        screen.getByRole("heading", { name: "Draw letters with letters." }),
      ).toBeVisible();
    });

    test("renders the English image-generation description", () => {
      renderScreen("en");

      expect(
        screen.getByText(
          "Combine your favorite characters and two colors to generate text art.",
        ),
      ).toBeVisible();
    });
  });

  test("renders the image-generation form in the page body", () => {
    renderScreen("ja");

    expect(
      screen.getByRole("textbox", { name: "描画する文字列" }),
    ).toBeVisible();
  });
});
