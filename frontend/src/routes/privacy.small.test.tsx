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

  test("フッターから日本語のプライバシーポリシーを開く", async () => {
    localStorage.setItem("locale", "ja");
    const { user } = openPage("/");

    await user.click(await screen.findByRole("link", { name: "プライバシーポリシー" }));

    expect(await screen.findByRole("heading", { name: "プライバシーポリシー" })).toBeVisible();
  });

  test("フッターから英語のプライバシーポリシーを開く", async () => {
    localStorage.setItem("locale", "en");
    const { user } = openPage("/");

    await user.click(await screen.findByRole("link", { name: "Privacy Policy" }));

    expect(await screen.findByRole("heading", { name: "Privacy Policy" })).toBeVisible();
  });

  test("日本語でお問い合わせ情報の取扱いを確認する", async () => {
    localStorage.setItem("locale", "ja");
    openPage("/privacy");

    expect(await screen.findByText(/Googleフォーム.*お名前.*お問い合わせ内容/)).toBeVisible();
    expect(screen.getByText(/お問い合わせへの対応.*サービスの改善/)).toBeVisible();
  });

  test("日本語でアクセス解析とCookieの取扱いを確認する", async () => {
    localStorage.setItem("locale", "ja");
    openPage("/privacy");

    expect(await screen.findByText(/Googleアナリティクス.*Cookie/)).toBeVisible();
    expect(screen.getByText(/ブラウザ.*Cookie.*制限/)).toBeVisible();
  });

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
