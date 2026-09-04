import { screen } from "@testing-library/react";
import { afterEach, describe, expect, test, vi } from "vitest";
import { setup } from "@/tests/test-utils";
import ErrorFallback from "./ErrorFallback";

afterEach(() => {
  vi.restoreAllMocks();
  localStorage.clear();
});

describe("ErrorFallback", () => {
  describe("localized fallback content", () => {
    // ID: ERROR-FALLBACK-S-001
    // Source: docs/v1/ui/ui.md § 20; docs/v1/ui/components/ErrorFallback.md § Screen specification
    // Given: The fallback is rendered with the default Japanese locale
    // When: An unexpected rendering error is shown
    // Then: The Japanese heading, recovery description, and reload action are available to the user
    // Blocked by: ErrorFallback implementation
    // Priority: P0
    test("renders the Japanese unexpected-error recovery content", () => {
      localStorage.clear();
      Object.defineProperty(navigator, "languages", {
        configurable: true,
        value: ["ja"],
      });

      setup(<ErrorFallback />);

      expect(
        screen.getByRole("heading", { name: "エラーが発生しました" }),
      ).toBeVisible();
    });

    // ID: ERROR-FALLBACK-S-002
    // Source: docs/v1/ui/ui.md § 20; docs/v1/ui/components/ErrorFallback.md § i18n
    // Given: The fallback resolves the supported English locale
    // When: An unexpected rendering error is shown
    // Then: The English heading, recovery description, and reload action are available to the user
    // Blocked by: ErrorFallback implementation
    // Priority: P1
    test("renders the English unexpected-error recovery content", () => {
      Object.defineProperty(navigator, "languages", {
        configurable: true,
        value: ["en-US"],
      });

      setup(<ErrorFallback />);

      expect(
        screen.getByRole("heading", { name: "An error occurred" }),
      ).toBeVisible();
      expect(
        screen.getByText(
          "Something unexpected happened. Please reload the page and try again.",
        ),
      ).toBeVisible();
      expect(
        screen.getByRole("button", { name: "Reload page" }),
      ).toBeEnabled();
    });
  });

  describe("locale resolution", () => {
    // ID: ERROR-FALLBACK-S-003
    // Source: docs/v1/ui/ui.md § 20; docs/v1/ui/components/ErrorFallback.md § i18n
    // Given: localStorage contains a supported locale and the browser reports another locale
    // When: The fallback resolves its display locale
    // Then: The stored supported locale determines the displayed copy
    // Blocked by: ErrorFallback implementation
    // Priority: P0
    test("prefers the stored supported locale over the browser locale", () => {
      localStorage.setItem("locale", "en");
      Object.defineProperty(navigator, "languages", {
        configurable: true,
        value: ["ja"],
      });

      setup(<ErrorFallback />);

      expect(
        screen.getByRole("heading", { name: "An error occurred" }),
      ).toBeVisible();
    });

    // ID: ERROR-FALLBACK-S-004
    // Source: docs/v1/ui/ui.md § 20; docs/v1/ui/components/ErrorFallback.md § i18n
    // Given: localStorage is absent or unsupported and the browser reports a supported language tag
    // When: The fallback resolves its display locale
    // Then: The first supported browser language determines the displayed copy
    // Blocked by: ErrorFallback implementation
    // Priority: P1
    test("uses the first supported browser locale when storage has no supported locale", () => {
      localStorage.clear();
      Object.defineProperty(navigator, "languages", {
        configurable: true,
        value: ["fr-FR", "en-US", "ja-JP"],
      });

      setup(<ErrorFallback />);

      expect(
        screen.getByRole("heading", { name: "An error occurred" }),
      ).toBeVisible();
    });

    // ID: ERROR-FALLBACK-S-005
    // Source: docs/v1/ui/ui.md § 20; docs/v1/ui/components/ErrorFallback.md § i18n
    // Given: localStorage cannot be read and no supported browser locale is available
    // When: The fallback resolves its display locale
    // Then: The fallback remains usable and displays the Japanese default copy
    // Blocked by: ErrorFallback implementation
    // Priority: P0
    test("falls back to Japanese when locale sources are unreadable or unsupported", () => {
      vi.spyOn(Storage.prototype, "getItem").mockImplementation(() => {
        throw new Error("storage unavailable");
      });
      Object.defineProperty(navigator, "languages", {
        configurable: true,
        value: ["fr-FR"],
      });

      setup(<ErrorFallback />);

      expect(
        screen.getByRole("heading", { name: "エラーが発生しました" }),
      ).toBeVisible();
    });
  });

  describe("recovery action", () => {
    // ID: ERROR-FALLBACK-S-006
    // Source: docs/v1/ui/ui.md § 20; docs/v1/ui/components/ErrorFallback.md § Screen specification
    // Given: The unexpected-error fallback is displayed
    // When: The user activates the reload action
    // Then: A normal browser page reload is requested without client-side navigation or an API request
    // Blocked by: ErrorFallback implementation
    // Priority: P0
    test.todo("reloads the page when the recovery action is activated");
  });
});
