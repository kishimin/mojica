import { act, screen, waitFor, within } from "@testing-library/react";
import { http, HttpResponse } from "msw";
import { afterEach, describe, expect, test, vi } from "vitest";
import ImageGenerationForm from "./ImageGenerationForm";
import { getPostImagesMockHandler } from "@/api/endpoints/image/image.msw";
import { worker } from "@/api/mocks/browser";
import { setupWithProviders } from "@/tests/test-utils";
import type { Locale } from "@/types/i18n";

const setupImageGenerationForm = (locale: Locale) =>
  setupWithProviders(<ImageGenerationForm locale={locale} />, locale);

afterEach(() => {
  vi.restoreAllMocks();
});

describe("ImageGenerationForm", () => {
  describe("initial rendering", () => {
    test("shows the empty preview before generating an image", () => {
      setupImageGenerationForm("ja");

      expect(screen.getByText("生成するとここに表示されます")).toBeVisible();
    });

    test("renders the image-generation form in Japanese", () => {
      setupImageGenerationForm("ja");

      expect(
        screen.getByRole("textbox", { name: "描画する文字列" }),
      ).toHaveValue("");
      expect(
        screen.getByRole("textbox", { name: "描画に使う文字" }),
      ).toHaveValue("");
      expect(screen.getByLabelText("描画に使う文字の色を選択")).toHaveValue(
        "#000000",
      );
      expect(
        screen.getByRole("textbox", { name: "敷き詰める文字" }),
      ).toHaveValue("");
      expect(screen.getByLabelText("敷き詰める文字の色を選択")).toHaveValue(
        "#ffffff",
      );
      expect(
        screen.getByRole("combobox", { name: "画像タイプ" }),
      ).toHaveTextContent("標準画像");
      expect(
        screen.getByRole("button", { name: "画像を生成する" }),
      ).toBeEnabled();
      expect(
        screen.getByText("生成した画像を確認してからダウンロードできます。"),
      ).toBeVisible();
    });

    test("renders the image-generation form in English", () => {
      setupImageGenerationForm("en");

      expect(
        screen.getByRole("textbox", { name: "Text to render" }),
      ).toHaveValue("");
      expect(
        screen.getByRole("textbox", {
          name: "Character used to render text",
        }),
      ).toHaveValue("");
      expect(
        screen.getByLabelText("Choose foreground character color"),
      ).toHaveValue("#000000");
      expect(
        screen.getByRole("textbox", { name: "Background character" }),
      ).toHaveValue("");
      expect(
        screen.getByLabelText("Choose background character color"),
      ).toHaveValue("#ffffff");
      expect(
        screen.getByRole("combobox", { name: "Image type" }),
      ).toHaveTextContent("Standard image");
      expect(
        screen.getByRole("button", { name: "Generate image" }),
      ).toBeEnabled();
      expect(
        screen.getByText(
          "Preview the generated image before downloading it.",
        ),
      ).toBeVisible();
    });
  });

  describe("client submission", () => {
    test("discards a response when inputs changed during generation", async () => {
      const callbacks = { requestStarted: () => {}, releaseResponse: () => {} };
      const requestStarted = new Promise<void>((resolve) => {
        callbacks.requestStarted = resolve;
      });
      const responseReady = new Promise<void>((resolve) => {
        callbacks.releaseResponse = resolve;
      });
      worker.use(getPostImagesMockHandler(async () => {
        callbacks.requestStarted();
        await responseReady;
        return new Uint8Array([137, 80]).buffer;
      }));
      const { user } = setupImageGenerationForm("ja");
      await user.type(screen.getByRole("textbox", { name: "描画する文字列" }), "KA");
      await user.type(screen.getByRole("textbox", { name: "描画に使う文字" }), "A");
      await user.type(screen.getByRole("textbox", { name: "敷き詰める文字" }), "B");
      await user.click(screen.getByRole("button", { name: "画像を生成する" }));
      await requestStarted;
      await user.type(screen.getByRole("textbox", { name: "描画する文字列" }), "X");
      callbacks.releaseResponse();
      await screen.findByRole("button", { name: "画像を生成する" });
      expect(screen.queryByRole("img", { name: "生成した画像" })).not.toBeInTheDocument();
      expect(screen.queryByRole("button", { name: "PNGをダウンロード" })).not.toBeInTheDocument();
    });
    test("uploads an image and renders the returned preview", async () => {
      const requests: FormData[] = [];
      vi.spyOn(URL, "createObjectURL").mockReturnValue("blob:preview");
      vi.spyOn(URL, "revokeObjectURL").mockImplementation(() => undefined);
      worker.use(http.post("*/images/frame-file", async ({ request }) => {
        requests.push(await request.formData());
        return new HttpResponse(new Uint8Array([137, 80]), { headers: { "Content-Type": "image/png" } });
      }));
      const { user } = setupImageGenerationForm("ja");
      await user.click(screen.getByRole("radio", { name: "画像を文字で描く" }));
      expect(screen.getByText(/縦横204ピクセル以下/)).toBeVisible();
      await user.upload(screen.getByLabelText("元画像"), new File(["test"], "input.png", { type: "image/png" }));
      await user.type(screen.getByRole("textbox", { name: "絵柄に使う文字" }), "A");
      await user.type(screen.getByRole("textbox", { name: "背景に使う文字" }), "B");
      await user.click(screen.getByRole("button", { name: "画像を生成する" }));
      expect(await screen.findByRole("img", { name: "生成した画像" })).toBeVisible();
      expect(requests).toHaveLength(1);
      expect(requests[0].get("frameImage")).toBeInstanceOf(File);
      expect(requests[0].get("foregroundCharacter")).toBe("A");
      expect(requests[0].get("backgroundCharacter")).toBe("B");
      expect(screen.queryByRole("combobox", { name: "画像タイプ" })).not.toBeInTheDocument();
      await user.type(screen.getByRole("textbox", { name: "絵柄に使う文字" }), "C");
      expect(screen.queryByRole("img", { name: "生成した画像" })).not.toBeInTheDocument();
    });

    test("submits a valid image-generation request once", async () => {
      const requests: unknown[] = [];
      worker.use(
        getPostImagesMockHandler(async ({ request }) => {
          requests.push(await request.json());
          return new ArrayBuffer(0);
        }),
      );

      const { user } = setupImageGenerationForm("ja");

      await user.type(
        screen.getByRole("textbox", { name: "描画する文字列" }),
        "KA",
      );
      await user.type(
        screen.getByRole("textbox", { name: "描画に使う文字" }),
        "🌻",
      );
      await user.type(
        screen.getByRole("textbox", { name: "敷き詰める文字" }),
        "☀",
      );
      await user.click(screen.getByRole("button", { name: "画像を生成する" }));

      await waitFor(() => {
        expect(requests).toEqual([
          {
            text: "KA",
            foregroundCharacter: "🌻",
            foregroundColor: "#000000",
            backgroundCharacter: "☀",
            backgroundColor: "#FFFFFF",
            type: "standard",
          },
        ]);
      });
    });

    test("shows the generated image in the preview", async () => {
      const png = Uint8Array.from(
        atob("iVBORw0KGgoAAAANSUhEUgAAAAoAAAAKCAYAAACNMs+9AAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAASSURBVChTY2CgJfiPLjAKiAYAPG0BAOp2pS8AAAAASUVORK5CYII="),
        (character) => character.charCodeAt(0),
      );
      worker.use(getPostImagesMockHandler(() => png.buffer));
      const { user } = setupImageGenerationForm("ja");
      await user.type(screen.getByRole("textbox", { name: "描画する文字列" }), "KA");
      await user.type(screen.getByRole("textbox", { name: "描画に使う文字" }), "A");
      await user.type(screen.getByRole("textbox", { name: "敷き詰める文字" }), "B");
      await user.click(screen.getByRole("button", { name: "画像を生成する" }));

      const preview = await screen.findByRole("img", { name: "生成した画像" });
      expect(preview).toBeVisible();
      await waitFor(() => expect(preview).toHaveProperty("naturalWidth", 10));
    });

    test("clears the preview and download action after an input change", async () => {
      worker.use(getPostImagesMockHandler(() => new Uint8Array([137, 80]).buffer));
      const { user } = setupImageGenerationForm("ja");
      await user.type(screen.getByRole("textbox", { name: "描画する文字列" }), "KA");
      await user.type(screen.getByRole("textbox", { name: "描画に使う文字" }), "A");
      await user.type(screen.getByRole("textbox", { name: "敷き詰める文字" }), "B");
      await user.click(screen.getByRole("button", { name: "画像を生成する" }));
      await screen.findByRole("img", { name: "生成した画像" });
      await user.type(screen.getByRole("textbox", { name: "描画する文字列" }), "X");

      expect(screen.queryByRole("img", { name: "生成した画像" })).not.toBeInTheDocument();
      expect(screen.queryByRole("button", { name: "PNGをダウンロード" })).not.toBeInTheDocument();
    });

    test("blocks submission and displays client validation errors", async () => {
      const { user } = setupImageGenerationForm("ja");

      await user.click(screen.getByRole("button", { name: "画像を生成する" }));

      // Do not assert synchronously; resolver errors are published during a later state update.
      // Do not use findByRole; the textbox exists before its error state updates.
      await waitFor(() => {
        expect(
          screen.getByRole("textbox", { name: "描画する文字列" }),
        ).toHaveAccessibleErrorMessage("描画する文字列を入力してください。");
      });
    });
  });

  describe("change validation", () => {
    test("displays a validation error after an invalid value is entered", async () => {
      const { user } = setupImageGenerationForm("ja");
      const textField = screen.getByRole("textbox", {
        name: "描画する文字列",
      });

      await user.type(textField, " ");

      await waitFor(() => {
        expect(textField).toHaveAccessibleErrorMessage(
          "空白以外の文字を入力してください。",
        );
      });
    });

    test("displays a validation error after an invalid color is entered", async () => {
      const { user } = setupImageGenerationForm("ja");
      const colorField = screen.getByRole("textbox", {
        name: "描画に使う文字の色",
      });

      await user.clear(colorField);
      await user.type(colorField, "#GGGGGG");

      await waitFor(() => {
        expect(colorField).toHaveAccessibleErrorMessage(
          "描画に使う文字の色をHEXカラー形式（#RRGGBB）で指定してください。",
        );
      });
    });
  });

  describe("submission state", () => {
    test("prevents duplicate submissions while generating", async () => {
      const requests: unknown[] = [];
      const callbacks = { releaseResponse: () => {} };
      const responseReady = new Promise<void>((resolve) => {
        callbacks.releaseResponse = resolve;
      });
      worker.use(
        getPostImagesMockHandler(async ({ request }) => {
          requests.push(await request.json());
          // Do not complete the response yet; duplicate clicks must be checked while it is pending.
          // Do not use a fixed delay; release the response explicitly after that check.
          await responseReady;
          return new ArrayBuffer(0);
        }),
      );

      const { user } = setupImageGenerationForm("ja");
      await user.type(
        screen.getByRole("textbox", { name: "描画する文字列" }),
        "KA",
      );
      await user.type(
        screen.getByRole("textbox", { name: "描画に使う文字" }),
        "🌻",
      );
      await user.type(
        screen.getByRole("textbox", { name: "敷き詰める文字" }),
        "☀",
      );

      await user.click(screen.getByRole("button", { name: "画像を生成する" }));

      const submittingButton = await screen.findByRole("button", {
        name: "生成中...",
      });
      expect(submittingButton).toBeDisabled();

      await waitFor(() => expect(requests).toHaveLength(1));
      await user.click(submittingButton);
      expect(requests).toHaveLength(1);

      callbacks.releaseResponse();
    });
  });

  describe("API validation errors", () => {
    test("maps 422 field errors to their corresponding inputs", async () => {
      worker.use(
        http.post("*/images", () =>
          HttpResponse.json(
            {
              code: "VALIDATION_ERROR",
              message: "入力を確認してください。",
              errors: [
                { field: "text", message: "描画する文字列が不正です。" },
              ],
            },
            { status: 422 },
          ),
        ),
      );

      const { user } = setupImageGenerationForm("ja");
      await user.type(
        screen.getByRole("textbox", { name: "描画する文字列" }),
        "KA",
      );
      await user.type(
        screen.getByRole("textbox", { name: "描画に使う文字" }),
        "🌻",
      );
      await user.type(
        screen.getByRole("textbox", { name: "敷き詰める文字" }),
        "☀",
      );
      await user.click(screen.getByRole("button", { name: "画像を生成する" }));

      // Do not assert synchronously; setError publishes the server error during a later state update.
      // Do not use findByRole; the textbox exists before the server error is associated.
      await waitFor(() => {
        expect(
          screen.getByRole("textbox", { name: "描画する文字列" }),
        ).toHaveAccessibleErrorMessage("描画する文字列が不正です。");
      });
    });
  });

  describe("API error banners", () => {
    test.each([
      { locale: "ja", modeLabel: "画像を文字で描く", fileLabel: "元画像", foregroundLabel: "絵柄に使う文字", backgroundLabel: "背景に使う文字", submitLabel: "画像を生成する", guidance: "元画像の縦横のサイズを小さくして、もう一度お試しください。" },
      { locale: "en", modeLabel: "Draw images with text", fileLabel: "Source image", foregroundLabel: "Pattern characters", backgroundLabel: "Background characters", submitLabel: "Generate image", guidance: "Reduce the source image dimensions and try again." },
    ] as const)("explains an oversized output in $locale", async ({ locale, modeLabel, fileLabel, foregroundLabel, backgroundLabel, submitLabel, guidance }) => {
      worker.use(http.post("*/images/frame-file", () => HttpResponse.json({ code: "IMAGE_OUTPUT_SIZE_EXCEEDED", message: "Too large" }, { status: 422 })));
      const { user } = setupImageGenerationForm(locale);
      await user.click(screen.getByRole("radio", { name: modeLabel }));
      await user.upload(screen.getByLabelText(fileLabel), new File(["test"], "input.png", { type: "image/png" }));
      await user.type(screen.getByRole("textbox", { name: foregroundLabel }), "A");
      await user.type(screen.getByRole("textbox", { name: backgroundLabel }), "B");
      await user.click(screen.getByRole("button", { name: submitLabel }));
      expect(await screen.findByText(guidance)).toBeVisible();
      expect(screen.getByRole("textbox", { name: foregroundLabel })).toHaveValue("A");
    });
    test("displays an API error message in a form-level alert", async () => {
      worker.use(
        http.post("*/images", () =>
          HttpResponse.json(
            {
              code: "BAD_REQUEST",
              message: "リクエストを確認してください。",
            },
            { status: 400 },
          ),
        ),
      );

      const { user } = setupImageGenerationForm("ja");
      await user.type(
        screen.getByRole("textbox", { name: "描画する文字列" }),
        "KA",
      );
      await user.type(
        screen.getByRole("textbox", { name: "描画に使う文字" }),
        "🌻",
      );
      await user.type(
        screen.getByRole("textbox", { name: "敷き詰める文字" }),
        "☀",
      );
      await user.click(screen.getByRole("button", { name: "画像を生成する" }));

      const alert = await screen.findByRole("alert");
      expect(
        within(alert).getByText("リクエストを確認してください。"),
      ).toBeVisible();
    });

    test("displays the server-error banner for INTERNAL_SERVER_ERROR", async () => {
      worker.use(
        http.post("*/images", () =>
          HttpResponse.json(
            {
              code: "INTERNAL_SERVER_ERROR",
              message: "内部エラーが発生しました。",
            },
            { status: 500 },
          ),
        ),
      );

      const { user } = setupImageGenerationForm("ja");
      await user.type(
        screen.getByRole("textbox", { name: "描画する文字列" }),
        "KA",
      );
      await user.type(
        screen.getByRole("textbox", { name: "描画に使う文字" }),
        "🌻",
      );
      await user.type(
        screen.getByRole("textbox", { name: "敷き詰める文字" }),
        "☀",
      );
      await user.click(screen.getByRole("button", { name: "画像を生成する" }));

      const alert = await screen.findByRole("alert");
      expect(within(alert).getByText("サーバーエラー")).toBeVisible();
      expect(
        within(alert).getByText("内部エラーが発生しました。"),
      ).toBeVisible();
    });

    test("displays the image-generation-service banner for IMAGE_GENERATION_FAILED", async () => {
      worker.use(
        http.post("*/images", () =>
          HttpResponse.json(
            {
              code: "IMAGE_GENERATION_FAILED",
              message: "画像生成サービスでエラーが発生しました。",
            },
            { status: 502 },
          ),
        ),
      );

      const { user } = setupImageGenerationForm("ja");
      await user.type(
        screen.getByRole("textbox", { name: "描画する文字列" }),
        "KA",
      );
      await user.type(
        screen.getByRole("textbox", { name: "描画に使う文字" }),
        "🌻",
      );
      await user.type(
        screen.getByRole("textbox", { name: "敷き詰める文字" }),
        "☀",
      );
      await user.click(screen.getByRole("button", { name: "画像を生成する" }));

      const alert = await screen.findByRole("alert");
      expect(within(alert).getByText("画像生成サービスエラー")).toBeVisible();
      expect(
        within(alert).getByText("画像生成サービスでエラーが発生しました。"),
      ).toBeVisible();
    });

    test("displays the timeout banner for IMAGE_GENERATION_TIMEOUT", async () => {
      worker.use(
        http.post("*/images", () =>
          HttpResponse.json(
            {
              code: "IMAGE_GENERATION_TIMEOUT",
              message: "画像生成がタイムアウトしました。",
            },
            { status: 504 },
          ),
        ),
      );

      const { user } = setupImageGenerationForm("ja");
      await user.type(
        screen.getByRole("textbox", { name: "描画する文字列" }),
        "KA",
      );
      await user.type(
        screen.getByRole("textbox", { name: "描画に使う文字" }),
        "🌻",
      );
      await user.type(
        screen.getByRole("textbox", { name: "敷き詰める文字" }),
        "☀",
      );
      await user.click(screen.getByRole("button", { name: "画像を生成する" }));

      const alert = await screen.findByRole("alert");
      expect(within(alert).getByText("タイムアウト")).toBeVisible();
      expect(
        within(alert).getByText("画像生成がタイムアウトしました。"),
      ).toBeVisible();
    });
  });

  describe("rate-limit retry behavior", () => {
    test("enforces Retry-After before allowing a retry", async () => {
      // Do not use real timers; the countdown needs a deterministic clock while userEvent and MSW need native setTimeout.
      vi.useFakeTimers({ toFake: ["Date", "setInterval", "clearInterval"] });
      worker.use(
        http.post("*/images", () =>
          HttpResponse.json(
            {
              code: "RATE_LIMIT_EXCEEDED",
              message: "しばらくお待ちください。",
            },
            { status: 429, headers: { "Retry-After": "5" } },
          ),
        ),
      );

      const { user } = setupImageGenerationForm("ja");
      await user.type(
        screen.getByRole("textbox", { name: "描画する文字列" }),
        "KA",
      );
      await user.type(
        screen.getByRole("textbox", { name: "描画に使う文字" }),
        "🌻",
      );
      await user.type(
        screen.getByRole("textbox", { name: "敷き詰める文字" }),
        "☀",
      );
      await user.click(screen.getByRole("button", { name: "画像を生成する" }));

      const cooldownButton = await screen.findByRole("button", {
        name: "5秒後に再試行できます",
      });
      expect(cooldownButton).toBeDisabled();

      act(() => {
        vi.advanceTimersByTime(5000);
      });
      expect(
        screen.getByRole("button", { name: "画像を生成する" }),
      ).toBeEnabled();
      vi.useRealTimers();
    });

    test("allows an immediate retry when Retry-After is absent", async () => {
      worker.use(
        http.post("*/images", () =>
          HttpResponse.json(
            {
              code: "RATE_LIMIT_EXCEEDED",
              message: "しばらくお待ちください。",
            },
            { status: 429 },
          ),
        ),
      );

      const { user } = setupImageGenerationForm("ja");
      await user.type(
        screen.getByRole("textbox", { name: "描画する文字列" }),
        "KA",
      );
      await user.type(
        screen.getByRole("textbox", { name: "描画に使う文字" }),
        "🌻",
      );
      await user.type(
        screen.getByRole("textbox", { name: "敷き詰める文字" }),
        "☀",
      );
      await user.click(screen.getByRole("button", { name: "画像を生成する" }));

      expect(
        await screen.findByRole("button", { name: "画像を生成する" }),
      ).toBeEnabled();
    });
  });
});
