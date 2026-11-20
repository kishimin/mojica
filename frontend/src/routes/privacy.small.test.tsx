import { createMemoryHistory, RouterProvider } from "@tanstack/react-router";
import { screen } from "@testing-library/react";
import { describe, test } from "vitest";
import { AppProviders } from "@/app/providers/AppProviders";
import { createAppRouter } from "@/lib/router";
import { setup } from "@/tests/test-utils";

const openPage = (path: string) => {
  const history = createMemoryHistory({ initialEntries: [path] });
  return setup(<AppProviders><RouterProvider router={createAppRouter({ history })} /></AppProviders>);
};

describe("privacy policy", () => {
  test("日本語でプライバシーポリシーを開く", async () => {
    localStorage.setItem("locale", "ja");
    openPage("/privacy");

    expect(await screen.findByRole("heading", { name: "プライバシーポリシー" })).toBeVisible();
  });

  test("英語でプライバシーポリシーを開く", async () => {
    localStorage.setItem("locale", "en");
    openPage("/privacy");

    expect(await screen.findByRole("heading", { name: "Privacy Policy" })).toBeVisible();
  });

  // ID: PP-03
  // Source: Request to add a privacy policy to mojica
  // Arrange: The home page is displayed in Japanese.
  // Act: Follow the privacy policy link in the footer.
  // Assert: The Japanese privacy policy is displayed.
  // Priority: P0
  test.todo("フッターから日本語のプライバシーポリシーを開く");

  // ID: PP-04
  // Source: Request to support English
  // Arrange: The home page is displayed in English.
  // Act: Follow the privacy policy link in the footer.
  // Assert: The English privacy policy is displayed.
  // Priority: P0
  test.todo("フッターから英語のプライバシーポリシーを開く");

  // ID: PP-05
  // Source: Confirmed use of Google Forms for inquiries
  // Arrange: The privacy policy is displayed in Japanese.
  // Act: Read the inquiry section.
  // Assert: Google Forms and the use of inquiry information are described.
  // Priority: P0
  test.todo("日本語でお問い合わせ情報の取扱いを確認する");

  // ID: PP-06
  // Source: Confirmed use of Google Analytics
  // Arrange: The privacy policy is displayed in Japanese.
  // Act: Read the analytics section.
  // Assert: Google Analytics and cookies are described.
  // Priority: P0
  test.todo("日本語でアクセス解析とCookieの取扱いを確認する");

  // ID: PP-07
  // Source: Request for an English privacy policy
  // Arrange: The privacy policy is displayed in English.
  // Act: Read the inquiry and analytics sections.
  // Assert: Google Forms, Google Analytics, and cookies are described.
  // Priority: P0
  test.todo("英語でお問い合わせとアクセス解析の取扱いを確認する");

  // ID: PP-08
  // Source: Privacy inquiry destination
  // Arrange: The privacy policy is displayed.
  // Act: Follow the contact link in the footer.
  // Assert: The appropriate Google Forms URL is available for the active language.
  // Priority: P1
  test.todo("プライバシーに関する問い合わせ先を利用する");
});
