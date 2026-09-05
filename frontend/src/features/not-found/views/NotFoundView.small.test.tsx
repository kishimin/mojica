import { screen } from "@testing-library/react";
import { describe, expect, test } from "vitest";
import NotFoundView from "./NotFoundView";
import { setupWithI18n } from "@/tests/test-utils";

describe("NotFoundView", () => {
  describe("Japanese locale", () => {
    // ID: NOT-FOUND-VIEW-S-001
    // Source: docs/v1/ui/ui.md § 19; docs/v1/ui/components/NotFoundView.md § Tests
    // Given: The 404 view is rendered with the Japanese locale
    // When: A path that does not exist is displayed
    // Then: The 404 status, Japanese heading, and explanation are visible
    // Blocked by: NotFoundView implementation and locale integration
    // Priority: P0
    test("renders the Japanese not-found status and message", () => {
      setupWithI18n(<NotFoundView />);

      expect(screen.getByRole("heading", { name: "404" })).toBeVisible();
      expect(
        screen.getByRole("heading", { name: "ページが見つかりません" }),
      ).toBeVisible();
      expect(
        screen.getByText(
          "URLが正しいか確認するか、トップページへ戻ってください。",
        ),
      ).toBeVisible();
    });
  });

  describe("English locale", () => {
    // ID: NOT-FOUND-VIEW-S-002
    // Source: docs/v1/ui/ui.md § 19; docs/v1/ui/components/NotFoundView.md § Tests
    // Given: The 404 view is rendered with the English locale
    // When: A path that does not exist is displayed
    // Then: The 404 status, English heading, and explanation are visible
    // Blocked by: NotFoundView implementation and locale integration
    // Priority: P1
    test("renders the English not-found status and message", () => {
      setupWithI18n(<NotFoundView />, "en");

      expect(screen.getByRole("heading", { name: "404" })).toBeVisible();
      expect(
        screen.getByRole("heading", { name: "Page not found" }),
      ).toBeVisible();
      expect(
        screen.getByText("Please check the URL or return to the homepage."),
      ).toBeVisible();
    });
  });

  describe("home navigation", () => {
    // ID: NOT-FOUND-VIEW-S-003
    // Source: docs/v1/ui/ui.md § 19; docs/v1/ui/components/NotFoundView.md § Tests
    // Given: The 404 view is displayed
    // When: The user inspects the recovery action
    // Then: A localized link back to the home page points to "/"
    // Blocked by: NotFoundView implementation and router link integration
    // Priority: P0
    test("provides an accessible link back to the home page", () => {
      setupWithI18n(<NotFoundView />);

      expect(
        screen.getByRole("link", { name: "トップページへ戻る" }),
      ).toHaveAttribute("href", "/");
    });

    test("provides the English accessible link back to the home page", () => {
      setupWithI18n(<NotFoundView />, "en");

      expect(
        screen.getByRole("link", { name: "Back to Home" }),
      ).toHaveAttribute("href", "/");
    });
  });
});
