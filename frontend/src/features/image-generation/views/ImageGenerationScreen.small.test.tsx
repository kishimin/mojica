import { screen } from "@testing-library/react";
import { describe, expect, test } from "vitest";
import ImageGenerationScreen from "./ImageGenerationScreen";
import { setupWithProviders } from "@/tests/test-utils";
import type { Locale } from "@/types/i18n";

const renderScreen = (locale: Locale) =>
  setupWithProviders(<ImageGenerationScreen />, locale);

describe("ImageGenerationScreen", () => {
  describe("Japanese copy", () => {
    // ID: IMAGE-GENERATION-SCREEN-S-001
    // Source: docs/v1/ui/ui.md § 4
    // Given: The image-generation screen is displayed with the Japanese locale
    // When: The page body is rendered
    // Then: The localized heading from imageGenerationScreenMessages.heading is available to the user
    // Blocked by: ImageGenerationScreen implementation
    // Priority: P0
    test("renders the Japanese image-generation heading", () => {
      renderScreen("ja");

      expect(
        screen.getByRole("heading", { name: "文字で、文字を描く。" }),
      ).toBeVisible();
    });

    // ID: IMAGE-GENERATION-SCREEN-S-002
    // Source: docs/v1/ui/ui.md § 4
    // Given: The image-generation screen is displayed with the Japanese locale
    // When: The page body is rendered
    // Then: The localized description from imageGenerationScreenMessages.description is available to the user
    // Blocked by: ImageGenerationScreen implementation
    // Priority: P0
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
    // ID: IMAGE-GENERATION-SCREEN-S-003
    // Source: docs/v1/ui/ui.md § 4
    // Given: The image-generation screen is displayed with the English locale
    // When: The page body is rendered
    // Then: The localized heading from imageGenerationScreenMessages.heading is available to the user
    // Blocked by: ImageGenerationScreen implementation
    // Priority: P1
    test("renders the English image-generation heading", () => {
      renderScreen("en");

      expect(
        screen.getByRole("heading", { name: "Draw letters with letters." }),
      ).toBeVisible();
    });

    // ID: IMAGE-GENERATION-SCREEN-S-004
    // Source: docs/v1/ui/ui.md § 4
    // Given: The image-generation screen is displayed with the English locale
    // When: The page body is rendered
    // Then: The localized description from imageGenerationScreenMessages.description is available to the user
    // Blocked by: ImageGenerationScreen implementation
    // Priority: P1
    test("renders the English image-generation description", () => {
      renderScreen("en");

      expect(
        screen.getByText(
          "Combine your favorite characters and two colors to generate text art.",
        ),
      ).toBeVisible();
    });
  });

  // ID: IMAGE-GENERATION-SCREEN-S-005
  // Source: docs/v1/ui/components/ImageGenerationScreen.md § Responsibility
  // Given: The image-generation screen is displayed within the application providers
  // When: The page body is rendered
  // Then: The image-generation form is available to the user
  // Blocked by: ImageGenerationScreen implementation
  // Priority: P0
  test("renders the image-generation form in the page body", () => {
    renderScreen("ja");

    expect(
      screen.getByRole("textbox", { name: "描画する文字列" }),
    ).toBeVisible();
  });
});
