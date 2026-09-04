import { act, screen, waitFor, within } from "@testing-library/react";
import { http, HttpResponse } from "msw";
import { describe, expect, test, vi } from "vitest";
import ImageGenerationForm from "./ImageGenerationForm";
import { getPostImagesMockHandler } from "@/api/endpoints/image/image.msw";
import { worker } from "@/api/mocks/browser";
import { setupWithProviders } from "@/tests/test-utils";
import type { Locale } from "@/types/i18n";

const setupImageGenerationForm = (locale: Locale) =>
  setupWithProviders(<ImageGenerationForm locale={locale} />, locale);

describe("ImageGenerationForm", () => {
  describe("initial rendering", () => {
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
        screen.getByText("生成したPNG画像は自動でダウンロードされます。"),
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
          "The generated PNG image will download automatically.",
        ),
      ).toBeVisible();
    });
  });

  describe("client submission", () => {
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

    // ID: IMAGE-GENERATION-FORM-S-013
    // Source: docs/v1/ui/components/ImageGenerationForm.md § State model; docs/v1/ui/ui.md §10, §12
    // Given: A valid form submission receives a PNG response with a Content-Disposition filename
    // When: The image-generation request succeeds
    // Then: The generated image is automatically offered for download with the server-provided filename
    // Priority: P0
    test.todo("downloads a successful PNG response with its server filename");

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
      const resolveResponse =
        vi.fn<(value?: void | PromiseLike<void>) => void>();
      const responseReady = new Promise<void>((resolve) => {
        resolveResponse.mockImplementation(resolve);
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

      await user.click(submittingButton);
      expect(requests).toHaveLength(1);

      resolveResponse();
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
