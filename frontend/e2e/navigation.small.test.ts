import { test } from "@playwright/test";

test.describe("navigation", () => {
  test.skip("returns from the not-found view to the image-generation home", async () => {
    // ID: NAVIGATION-E2E-S-001
    // Source: docs/v1/ui/ui.md § 19; docs/v1/ui/components/NotFoundView.md
    // Given: The browser opens an unknown application path
    // When: The user activates the home recovery action
    // Then: The image-generation home screen is displayed
    // Blocked by: Root route and not-found route connection
    // Priority: P0
  });

  test.skip("uses the image-generation controls with keyboard input", async () => {
    // ID: NAVIGATION-E2E-S-002
    // Source: docs/v1/ui/ui.md § 4-7; docs/v1/ui/component-design.md § 4
    // Given: The image-generation home screen is displayed
    // When: The user reaches the controls and operates them with the keyboard
    // Then: The documented form flow remains usable without a pointer
    // Blocked by: Browser-level keyboard flow and control focus order
    // Priority: P1
  });
});
