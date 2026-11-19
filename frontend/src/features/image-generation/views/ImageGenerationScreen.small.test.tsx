import { screen, waitFor } from "@testing-library/react";
import { http, HttpResponse } from "msw";
import { describe, expect, test } from "vitest";
import ImageGenerationScreen from "./ImageGenerationScreen";
import { worker } from "@/api/mocks/browser";
import { setupWithProviders } from "@/tests/test-utils";
import type { Locale } from "@/types/i18n";

const renderScreen = (locale: Locale) =>
  setupWithProviders(<ImageGenerationScreen />, locale);

describe("ImageGenerationScreen", () => {
  test("画像モードへ切り替えられる", async () => {
    const { user } = renderScreen("ja");

    await user.click(screen.getByRole("radio", { name: "画像を文字で描く" }));

    expect(screen.getByLabelText("元画像")).toBeVisible();
    expect(screen.queryByRole("textbox", { name: "描画する文字列" })).not.toBeInTheDocument();
  });

  test("画像を選択せず生成するとエラーを表示する", async () => {
    const { user } = renderScreen("ja");
    await user.click(screen.getByRole("radio", { name: "画像を文字で描く" }));

    await user.click(screen.getByRole("button", { name: "画像を生成する" }));

    expect(screen.getByRole("alert")).toHaveTextContent("画像を選択してください");
  });

  test("選択画像と描画文字を送信する", async () => {
    const requests: FormData[] = [];
    worker.use(http.post("*/images/frame-file", async ({ request }) => {
      requests.push(await request.formData());
      return new HttpResponse(new Uint8Array([137, 80]), { headers: { "Content-Type": "image/png" } });
    }));
    const { user } = renderScreen("ja");
    await user.click(screen.getByRole("radio", { name: "画像を文字で描く" }));
    await user.upload(screen.getByLabelText("元画像"), new File(["image"], "frame.png", { type: "image/png" }));
    await user.type(screen.getByRole("textbox", { name: "絵柄に使う文字" }), "A");
    await user.type(screen.getByRole("textbox", { name: "背景に使う文字" }), "B");

    await user.click(screen.getByRole("button", { name: "画像を生成する" }));

    await waitFor(() => expect(requests).toHaveLength(1));
    expect(requests[0].get("frameImage")).toBeInstanceOf(File);
    expect(requests[0].get("foregroundCharacter")).toBe("A");
    expect(requests[0].get("backgroundCharacter")).toBe("B");
  });

  describe("Japanese copy", () => {
    test("renders the Japanese image-generation heading", () => {
      renderScreen("ja");

      expect(
        screen.getByRole("heading", { name: "文字で、文字を描く。" }),
      ).toBeVisible();
    });

    test("renders the Japanese image-generation description", () => {
      renderScreen("ja");

      expect(
        screen.getByText(
          "好きな文字と2つの色を組み合わせて、文字アート画像を生成します。",
        ),
      ).toBeVisible();
    });
  });

  describe("English copy", () => {
    test("renders the English image-generation heading", () => {
      renderScreen("en");

      expect(
        screen.getByRole("heading", { name: "Draw letters with letters." }),
      ).toBeVisible();
    });

    test("renders the English image-generation description", () => {
      renderScreen("en");

      expect(
        screen.getByText(
          "Combine your favorite characters and two colors to generate text art.",
        ),
      ).toBeVisible();
    });
  });

  test("renders the image-generation form in the page body", () => {
    renderScreen("ja");

    expect(
      screen.getByRole("textbox", { name: "描画する文字列" }),
    ).toBeVisible();
  });
});
