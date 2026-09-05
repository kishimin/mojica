import { expect, test } from "../fixtures.js";

test.describe("image generation", () => {
  test("generates an image through the real API", async ({
    imageGenerationPage,
  }) => {
    // ID: IMAGE-GENERATION-E2E-M-001
    // Source: docs/v1/ui/ui.md § 4-10; docs/v1/ui/components/ImageGenerationForm.md
    // Given: The frontend and the configured Mojica API are available
    // When: The user submits a valid image-generation request
    // Then: The user can obtain the generated image
    // Blocked by: Real Mojica API and Glyph Forge service lifecycle
    // Priority: P0
    const download = await imageGenerationPage.generateImage();

    expect(download.suggestedFilename()).toMatch(/\.png$/);
  });

  test.skip("shows a user-facing message for an image-generation API error", async () => {
    // ID: IMAGE-GENERATION-E2E-M-002
    // Source: docs/v1/ui/ui.md § 12; docs/v1/ui/components/ImageGenerationForm.md § State model
    // Given: The configured API returns a documented image-generation error
    // When: The user submits an image-generation request
    // Then: The corresponding localized error presentation is shown
    // Blocked by: A deterministic non-production API error scenario
    // Priority: P1
  });
});
