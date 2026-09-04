import { screen } from "@testing-library/react";
import { describe, expect, test } from "vitest";
import ImageGenerationScreen from "./ImageGenerationScreen";
import { AppProviders } from "@/app/providers/AppProviders";
import { setup } from "@/tests/test-utils";

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
      setup(
        <AppProviders>
          <ImageGenerationScreen />
        </AppProviders>,
      );

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
    test.todo("renders the Japanese image-generation description");
  });

  describe("English copy", () => {
    // ID: IMAGE-GENERATION-SCREEN-S-003
    // Source: docs/v1/ui/ui.md § 4
    // Given: The image-generation screen is displayed with the English locale
    // When: The page body is rendered
    // Then: The localized heading from imageGenerationScreenMessages.heading is available to the user
    // Blocked by: ImageGenerationScreen implementation
    // Priority: P1
    test.todo("renders the English image-generation heading");

    // ID: IMAGE-GENERATION-SCREEN-S-004
    // Source: docs/v1/ui/ui.md § 4
    // Given: The image-generation screen is displayed with the English locale
    // When: The page body is rendered
    // Then: The localized description from imageGenerationScreenMessages.description is available to the user
    // Blocked by: ImageGenerationScreen implementation
    // Priority: P1
    test.todo("renders the English image-generation description");
  });

  // ID: IMAGE-GENERATION-SCREEN-S-005
  // Source: docs/v1/ui/components/ImageGenerationScreen.md § Responsibility
  // Given: The image-generation screen is displayed within the application providers
  // When: The page body is rendered
  // Then: The image-generation form is available to the user
  // Blocked by: ImageGenerationScreen implementation
  // Priority: P0
  test.todo("renders the image-generation form in the page body");
});
