import { act, screen, waitFor } from "@testing-library/react";
import { http, HttpResponse } from "msw";
import { describe, expect, test, vi } from "vitest";
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

  test("選択した色を画像生成に使う", async () => {
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
    await user.clear(screen.getByRole("textbox", { name: "絵柄の文字色" }));
    await user.type(screen.getByRole("textbox", { name: "絵柄の文字色" }), "#55aaff");
    await user.click(screen.getByRole("button", { name: "画像を生成する" }));

    await waitFor(() => expect(requests).toHaveLength(1));
    expect(requests[0].get("foregroundColor")).toBe("#55aaff");
  });

  test("生成結果を表示してからダウンロードできる", async () => {
    worker.use(http.post("*/images/frame-file", () =>
      new HttpResponse(new Uint8Array([137, 80]), { headers: { "Content-Type": "image/png" } }),
    ));
    const { user } = renderScreen("ja");
    await user.click(screen.getByRole("radio", { name: "画像を文字で描く" }));
    await user.upload(screen.getByLabelText("元画像"), new File(["image"], "frame.png", { type: "image/png" }));
    await user.type(screen.getByRole("textbox", { name: "絵柄に使う文字" }), "A");
    await user.type(screen.getByRole("textbox", { name: "背景に使う文字" }), "B");
    await user.click(screen.getByRole("button", { name: "画像を生成する" }));

    expect(await screen.findByRole("img", { name: "生成した画像" })).toBeVisible();
    expect(screen.getByRole("button", { name: "PNGをダウンロード" })).toBeEnabled();
  });

  test("生成中に入力が変わったら古い画像を表示しない", async () => {
    const callbacks = { started: () => {}, release: () => {} };
    const started = new Promise<void>((resolve) => { callbacks.started = resolve; });
    const ready = new Promise<void>((resolve) => { callbacks.release = resolve; });
    const createUrl = vi.spyOn(URL, "createObjectURL").mockReturnValue("blob:old");
    worker.use(http.post("*/images/frame-file", async () => {
      callbacks.started();
      await ready;
      return new HttpResponse(new Uint8Array([137, 80]), { headers: { "Content-Type": "image/png" } });
    }));
    const { user } = renderScreen("ja");
    await user.click(screen.getByRole("radio", { name: "画像を文字で描く" }));
    await user.upload(screen.getByLabelText("元画像"), new File(["image"], "frame.png", { type: "image/png" }));
    await user.type(screen.getByRole("textbox", { name: "絵柄に使う文字" }), "A");
    await user.type(screen.getByRole("textbox", { name: "背景に使う文字" }), "B");
    await user.click(screen.getByRole("button", { name: "画像を生成する" }));
    await started;
    expect(screen.getByRole("button", { name: "画像を生成する" })).toBeDisabled();
    await user.type(screen.getByRole("textbox", { name: "絵柄に使う文字" }), "C");
    await act(async () => { callbacks.release(); await ready; });
    await waitFor(() => expect(screen.getByRole("button", { name: "画像を生成する" })).toBeEnabled());
    expect(createUrl).not.toHaveBeenCalled();
    createUrl.mockRestore();
  });

  test("画像生成サービスのエラーを表示する", async () => {
    worker.use(http.post("*/images/frame-file", () =>
      HttpResponse.json({ code: "IMAGE_GENERATION_FAILED", message: "画像生成に失敗しました。" }, { status: 502 }),
    ));
    const { user } = renderScreen("ja");
    await user.click(screen.getByRole("radio", { name: "画像を文字で描く" }));
    await user.upload(screen.getByLabelText("元画像"), new File(["image"], "frame.png", { type: "image/png" }));
    await user.type(screen.getByRole("textbox", { name: "絵柄に使う文字" }), "A");
    await user.type(screen.getByRole("textbox", { name: "背景に使う文字" }), "B");
    await user.click(screen.getByRole("button", { name: "画像を生成する" }));

    expect(await screen.findByRole("alert")).toHaveTextContent("画像生成に失敗しました。");
  });

  test("描画文字が空の場合は送信せず入力エラーを表示する", async () => {
    const requests: FormData[] = [];
    worker.use(http.post("*/images/frame-file", async ({ request }) => {
      requests.push(await request.formData());
      return new HttpResponse(new Uint8Array([137, 80]), { headers: { "Content-Type": "image/png" } });
    }));
    const { user } = renderScreen("ja");
    await user.click(screen.getByRole("radio", { name: "画像を文字で描く" }));
    await user.upload(screen.getByLabelText("元画像"), new File(["image"], "frame.png", { type: "image/png" }));
    await user.click(screen.getByRole("button", { name: "画像を生成する" }));

    expect(screen.getByText("描画に使う文字を入力してください。")).toBeVisible();
    expect(screen.getByText("敷き詰める文字を入力してください。")).toBeVisible();
    expect(requests).toHaveLength(0);
  });

  test("出力サイズが大きい場合は元画像について案内する", async () => {
    worker.use(http.post("*/images/frame-file", () =>
      HttpResponse.json({ code: "IMAGE_OUTPUT_SIZE_EXCEEDED", message: "出力上限を超えました。" }, { status: 422 }),
    ));
    const { user } = renderScreen("ja");
    await user.click(screen.getByRole("radio", { name: "画像を文字で描く" }));
    await user.upload(screen.getByLabelText("元画像"), new File(["image"], "frame.png", { type: "image/png" }));
    await user.type(screen.getByRole("textbox", { name: "絵柄に使う文字" }), "A");
    await user.type(screen.getByRole("textbox", { name: "背景に使う文字" }), "B");
    await user.click(screen.getByRole("button", { name: "画像を生成する" }));

    expect(await screen.findByRole("alert")).toHaveTextContent("元画像の縦横のサイズを小さくして、もう一度お試しください。");
  });

  test("画像生成の回数制限中は再試行を待つ", async () => {
    vi.useFakeTimers({ toFake: ["Date", "setInterval", "clearInterval"] });
    worker.use(http.post("*/images/frame-file", () =>
      HttpResponse.json({ code: "RATE_LIMIT_EXCEEDED", message: "しばらくお待ちください。" }, { status: 429, headers: { "Retry-After": "5" } }),
    ));
    const { user } = renderScreen("ja");
    await user.click(screen.getByRole("radio", { name: "画像を文字で描く" }));
    await user.upload(screen.getByLabelText("元画像"), new File(["image"], "frame.png", { type: "image/png" }));
    await user.type(screen.getByRole("textbox", { name: "絵柄に使う文字" }), "A");
    await user.type(screen.getByRole("textbox", { name: "背景に使う文字" }), "B");
    await user.click(screen.getByRole("button", { name: "画像を生成する" }));
    expect(await screen.findByRole("button", { name: "5秒後に再試行できます" })).toBeDisabled();
    act(() => { vi.advanceTimersByTime(5000); });
    expect(screen.getByRole("button", { name: "画像を生成する" })).toBeEnabled();
    vi.useRealTimers();
  });

  test("英語の画像モードで対応形式を案内する", async () => {
    const { user } = renderScreen("en");
    await user.click(screen.getByRole("radio", { name: "Draw images with text" }));

    expect(screen.getByLabelText("Source image")).toBeVisible();
    expect(screen.getByText(/PNG, JPEG, or WebP up to 2 MiB/)).toBeVisible();
    expect(screen.queryByText(/pixels? per side/i)).not.toBeInTheDocument();
  });

  test("画像生成APIのフィールドエラーを入力欄に表示する", async () => {
    worker.use(http.post("*/images/frame-file", () =>
      HttpResponse.json({ code: "VALIDATION_ERROR", message: "入力を確認してください。", errors: [{ field: "foregroundCharacter", message: "絵柄の文字を確認してください。" }] }, { status: 422 }),
    ));
    const { user } = renderScreen("ja");
    await user.click(screen.getByRole("radio", { name: "画像を文字で描く" }));
    await user.upload(screen.getByLabelText("元画像"), new File(["image"], "frame.png", { type: "image/png" }));
    await user.type(screen.getByRole("textbox", { name: "絵柄に使う文字" }), "A");
    await user.type(screen.getByRole("textbox", { name: "背景に使う文字" }), "B");
    await user.click(screen.getByRole("button", { name: "画像を生成する" }));

    expect(await screen.findByText("絵柄の文字を確認してください。")).toBeVisible();
    expect(screen.getByRole("textbox", { name: "絵柄に使う文字" })).toHaveAttribute("aria-invalid", "true");
  });

  test("生成前はプレビュー案内と無効なダウンロードボタンを表示する", async () => {
    const { user } = renderScreen("ja");
    await user.click(screen.getByRole("radio", { name: "画像を文字で描く" }));

    expect(screen.getByText("生成するとここに表示されます")).toBeVisible();
    expect(screen.getByRole("button", { name: "PNGをダウンロード" })).toBeDisabled();
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
