import { test } from "@playwright/test";

test.describe("error fallback", () => {
  test.skip("keeps the unexpected-error fallback usable in Japanese", async () => {
    // ID: ERROR-FALLBACK-E2E-S-001
    // Source: docs/v1/ui/ui.md § 20; docs/v1/ui/components/ErrorFallback.md
    // Given: The application reaches the unexpected-error fallback with Japanese locale resolution
    // When: The user inspects the recovery screen
    // Then: The localized recovery content is available
    // Blocked by: A browser-level route to the unexpected-error boundary
    // Priority: P0
  });

  test.skip("keeps the unexpected-error fallback usable in English", async () => {
    // ID: ERROR-FALLBACK-E2E-S-002
    // Source: docs/v1/ui/ui.md § 20; docs/v1/ui/components/ErrorFallback.md
    // Given: The application reaches the unexpected-error fallback with English locale resolution
    // When: The user inspects the recovery screen
    // Then: The English recovery content is available
    // Blocked by: A browser-level route to the unexpected-error boundary
    // Priority: P1
  });
});

test.describe("responsive layout", () => {
  test.skip("remains usable at the documented viewport widths", async () => {
    // ID: RESPONSIVE-E2E-S-001
    // Source: docs/v1/ui/ui.md § 3; docs/v1/ui/component-design.md § 2
    // Given: The image-generation screen is opened at 390px, 768px, and 1440px widths
    // When: The user views and operates the screen at each documented viewport
    // Then: The documented controls remain usable without unintended horizontal scrolling
    // Blocked by: Viewport project configuration and responsive layout verification
    // Priority: P1
  });
});
