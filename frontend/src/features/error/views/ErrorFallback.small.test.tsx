import { screen } from "@testing-library/react";
import { afterEach, beforeEach, describe, expect, test, vi } from "vitest";
import ErrorFallback from "./ErrorFallback";
import { setup } from "@/tests/test-utils";

beforeEach(() => {
  localStorage.clear();
});

afterEach(() => {
  vi.restoreAllMocks();
});

const mockBrowserLanguages = (languages: string[]) => {
  vi.spyOn(navigator, "languages", "get").mockReturnValue(languages);
};

describe("ErrorFallback", () => {
  describe("localized fallback content", () => {
    test("renders the Japanese unexpected-error recovery content", () => {
      mockBrowserLanguages(["ja"]);

      setup(<ErrorFallback />);

      expect(
        screen.getByRole("heading", { name: "エラーが発生しました" }),
      ).toBeVisible();
      expect(
        screen.getByText(
          "予期しない問題が発生しました。しばらくしてからページを再読み込みしてください。",
        ),
      ).toBeVisible();
      expect(
        screen.getByRole("button", { name: "ページを再読み込み" }),
      ).toBeEnabled();
    });

    test("renders the English unexpected-error recovery content", () => {
      mockBrowserLanguages(["en-US"]);

      setup(<ErrorFallback />);

      expect(
        screen.getByRole("heading", { name: "An error occurred" }),
      ).toBeVisible();
      expect(
        screen.getByText(
          "Something unexpected happened. Please reload the page and try again.",
        ),
      ).toBeVisible();
      expect(screen.getByRole("button", { name: "Reload page" })).toBeEnabled();
    });
  });

  describe("locale resolution", () => {
    test("prefers the stored supported locale over the browser locale", () => {
      localStorage.setItem("locale", "en");
      mockBrowserLanguages(["ja"]);

      setup(<ErrorFallback />);

      expect(
        screen.getByRole("heading", { name: "An error occurred" }),
      ).toBeVisible();
    });

    test("uses the first supported browser locale when storage has no supported locale", () => {
      mockBrowserLanguages(["fr-FR", "en-US", "ja-JP"]);

      setup(<ErrorFallback />);

      expect(
        screen.getByRole("heading", { name: "An error occurred" }),
      ).toBeVisible();
    });

    test("falls back to Japanese when locale sources are unreadable or unsupported", () => {
      vi.spyOn(Storage.prototype, "getItem").mockImplementation(() => {
        throw new Error("storage unavailable");
      });
      mockBrowserLanguages(["fr-FR"]);

      setup(<ErrorFallback />);

      expect(
        screen.getByRole("heading", { name: "エラーが発生しました" }),
      ).toBeVisible();
    });
  });
});
