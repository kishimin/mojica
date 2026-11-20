import { createMemoryHistory, RouterProvider } from "@tanstack/react-router";
import { screen } from "@testing-library/react";
import { describe, expect, test } from "vitest";
import { AppProviders } from "@/app/providers/AppProviders";
import { createAppRouter } from "@/lib/router";
import { setup } from "@/tests/test-utils";

const openPage = (path: string) => {
  const history = createMemoryHistory({ initialEntries: [path] });
  return setup(<AppProviders><RouterProvider router={createAppRouter({ history })} /></AppProviders>);
};

describe("privacy policy", () => {
  test.each([
    ["ja", "トップページへ戻る", "文字で、文字を描く。"],
    ["en", "Back to home", "Draw letters with letters."],
  ])("%sのプライバシーポリシーからトップページへ戻る", async (locale, link, heading) => {
    localStorage.setItem("locale", locale);
    const { user } = openPage("/privacy");

    await user.click(await screen.findByRole("link", { name: link }));

    expect(await screen.findByRole("heading", { name: heading })).toBeVisible();
  });

  test("英語でタグマネージャーの利用を説明する", async () => {
    localStorage.setItem("locale", "en");
    openPage("/privacy");

    expect(await screen.findByText("This site uses Google Analytics through Google Tag Manager to understand usage and improve the service.")).toBeVisible();
  });

  test("日本語でタグマネージャーの利用を説明する", async () => {
    localStorage.setItem("locale", "ja");
    openPage("/privacy");

    expect(await screen.findByText("本サイトでは、利用状況の把握とサービスの改善のため、Googleタグマネージャーを通じてGoogleアナリティクスを使用しています。")).toBeVisible();
  });

  test.each([
    ["ja", "Googleのプライバシーポリシー"],
    ["en", "Google Privacy Policy"],
  ])("%sでGoogleのプライバシーポリシーへのリンクを表示する", async (locale, label) => {
    localStorage.setItem("locale", locale);
    openPage("/privacy");

    expect(await screen.findByRole("link", { name: label })).toHaveAttribute(
      "href", `https://policies.google.com/privacy?hl=${locale}`,
    );
  });

  test("英語で画像生成時に入力を送信することを説明する", async () => {
    localStorage.setItem("locale", "en");
    openPage("/privacy");

    expect(await screen.findByText("When you generate an image, your text and settings are sent to a server for processing.")).toBeVisible();
  });

  test("画像生成時に入力を送信することを説明する", async () => {
    localStorage.setItem("locale", "ja");
    openPage("/privacy");

    expect(await screen.findByText("画像の生成時には、入力された文字や設定が生成処理のためにサーバーへ送信されます。")).toBeVisible();
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

    expect(await screen.findByText("お問い合わせにはGoogleフォームを利用します。フォームに入力されたお名前（ニックネームを含む）やお問い合わせ内容は、お問い合わせへの対応とサービスの改善に利用します。")).toBeVisible();
  });

  test("日本語でアクセス解析とCookieの取扱いを確認する", async () => {
    localStorage.setItem("locale", "ja");
    openPage("/privacy");

    expect(await screen.findByText("GoogleアナリティクスはCookieなどを利用して利用状況を収集します。ブラウザの設定からCookieの保存を制限または削除できます。")).toBeVisible();
  });

  test("英語でお問い合わせとアクセス解析の取扱いを確認する", async () => {
    localStorage.setItem("locale", "en");
    openPage("/privacy");

    expect(await screen.findByText("We use Google Forms for inquiries. We use the name you provide, including a nickname, and your inquiry to respond and improve the service.")).toBeVisible();
    expect(screen.getByText("Google Analytics uses cookies to collect usage information. You can restrict or delete cookies through your browser settings.")).toBeVisible();
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
