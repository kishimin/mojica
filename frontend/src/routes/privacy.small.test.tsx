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
  test("英語で画像生成時に入力を送信することを説明する", async () => {
    localStorage.setItem("locale", "en");
    openPage("/privacy");

    expect(await screen.findByText(/text and settings.*sent to a server/i)).toBeVisible();
  });

  test("画像生成時に入力を送信することを説明する", async () => {
    localStorage.setItem("locale", "ja");
    openPage("/privacy");

    expect(await screen.findByText(/文字.*設定.*サーバー.*送信/)).toBeVisible();
  });

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

  test("英語でお問い合わせとアクセス解析の取扱いを確認する", async () => {
    localStorage.setItem("locale", "en");
    openPage("/privacy");

    expect(await screen.findByText(/Google Forms.*inquiries/)).toBeVisible();
    expect(screen.getByText(/Google Analytics.*cookies/i)).toBeVisible();
  });

  test.each([
    ["ja", "お問い合わせ", "1FAIpQLSdG8CcjnmGBseY6ExWhEidXutkXzyU3VmVrhFfQs9crJeOndQ"],
    ["en", "Contact", "1FAIpQLSedj8RXI0wSVnIHYu4jyRi7oyZ9jRFH98QGLyXAHtHC10XqFw"],
  ])("%sでプライバシーに関する問い合わせ先を利用する", async (locale, label, formId) => {
    localStorage.setItem("locale", locale);
    openPage("/privacy");

    expect(await screen.findByRole("link", { name: label })).toHaveAttribute(
      "href", `https://docs.google.com/forms/d/e/${formId}/viewform`,
    );
  });
});
