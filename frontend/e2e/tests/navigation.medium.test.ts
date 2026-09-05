import { test } from "../fixtures.js";

test.describe("navigation", () => {
  test.skip("returns from the not-found view to the image-generation home", async () => {
    // ID: NAVIGATION-E2E-M-001
    // Source: docs/v1/ui/ui.md § 19; docs/v1/ui/components/NotFoundView.md
    // Given: The browser opens an unknown application path
    // When: The user activates the home recovery action
    // Then: The image-generation home screen is displayed
    // Blocked by: Root route and not-found route connection
    // Priority: P0
  });
});
