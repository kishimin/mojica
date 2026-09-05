import { screen } from "@testing-library/react";
import { describe, expect, test } from "vitest";
import NotFoundView from "./NotFoundView";
import { setupWithI18n } from "@/tests/test-utils";

describe("NotFoundView", () => {
  describe("Japanese locale", () => {
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
