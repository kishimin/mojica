import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import { act, screen, waitFor } from "@testing-library/react";
import { http, HttpResponse } from "msw";
import { describe, expect, test, vi } from "vitest";
import ImageGenerationForm from "./ImageGenerationForm";
import { getPostImagesMockHandler } from "@/api/endpoints/image/image.msw";
import { worker } from "@/api/mocks/browser";
import { setupWithI18n } from "@/tests/test-utils";
import type { Locale } from "@/types/i18n";

const setupImageGenerationForm = (locale: Locale) =>
  setupWithI18n(
    <QueryClientProvider client={new QueryClient()}>
      <ImageGenerationForm locale={locale} />
    </QueryClientProvider>,
    locale,
  );

describe("ImageGenerationForm", () => {
  describe("initial rendering", () => {
    // ID: IMAGE-GENERATION-FORM-S-001
    // Source: docs/v1/ui/ui.md § 4-7; docs/v1/ui/components/ImageGenerationForm.md § State model
    // Given: The form is rendered with the Japanese locale
    // When: The image-generation screen is displayed
    // Then: The form exposes the required inputs, the standard image type, and the generate-image action with documented empty defaults
    // Blocked by: ImageGenerationForm implementation
    // Priority: P0
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
    });

    // ID: IMAGE-GENERATION-FORM-S-002
    // Source: docs/v1/ui/ui.md § 13; docs/v1/ui/components/ImageGenerationForm.md § Props
    // Given: The form is rendered with the English locale
    // When: The image-generation screen is displayed
    // Then: The form exposes English labels, options, and action text
    // Blocked by: ImageGenerationForm implementation and English i18n messages
    // Priority: P1
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
    });
  });

  describe("client submission", () => {
    // ID: IMAGE-GENERATION-FORM-S-003
    // Source: docs/v1/ui/ui.md § 8; docs/v1/ui/components/ImageGenerationForm.md § State model
    // Given: The user supplies a valid request and selects an image type
    // When: The user submits the form
    // Then: One POST /images request is sent with the entered values in the documented API shape
    // Blocked by: ImageGenerationForm implementation
    // Priority: P0
    test("submits a valid image-generation request once", async () => {
      const requests: unknown[] = [];
      worker.use(
        getPostImagesMockHandler(async ({ request }) => {
          requests.push(await request.json());
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

    // ID: IMAGE-GENERATION-FORM-S-004
    // Source: docs/v1/ui/ui.md § 11; docs/v1/ui/components/ImageGenerationForm.md § Validation schema
    // Given: A required form value is invalid
    // When: The user submits the form
    // Then: The corresponding validation message is displayed and POST /images is not called
    // Blocked by: ImageGenerationForm implementation
    // Priority: P0
    test("blocks submission and displays client validation errors", async () => {
      const { user } = setupImageGenerationForm("ja");

      await user.click(screen.getByRole("button", { name: "画像を生成する" }));

      // Do not use findByRole; the textbox already exists and only its error state updates later.
      await waitFor(() => {
        expect(
          screen.getByRole("textbox", { name: "描画する文字列" }),
        ).toHaveAccessibleErrorMessage("描画する文字列を入力してください。");
      });
    });
  });

  describe("change validation", () => {
    // ID: IMAGE-GENERATION-FORM-S-005
    // Source: docs/v1/ui/ui.md § 11; docs/v1/ui/components/ImageGenerationForm.md § Validation schema
    // Given: The form is rendered with an empty required text field
    // When: The user enters an invalid value into the text field
    // Then: The field becomes invalid and exposes the validation message as its accessible error
    // Blocked by: ImageGenerationForm implementation
    // Priority: P0
    test("displays a validation error after an invalid value is entered", async () => {
      const { user } = setupImageGenerationForm("ja");
      const textField = screen.getByRole("textbox", {
        name: "描画する文字列",
      });

      await user.type(textField, " ");

      await waitFor(() => {
        expect(textField).toHaveAccessibleErrorMessage(
          "描画する文字列を入力してください。",
        );
      });
    });
  });

  describe("submission state", () => {
    // ID: IMAGE-GENERATION-FORM-S-006
    // Source: docs/v1/ui/ui.md § 9; docs/v1/ui/components/ImageGenerationForm.md § State model
    // Given: A valid submission is in progress
    // When: The user presses the generate-image action again
    // Then: The action is unavailable and no duplicate POST /images request is sent
    // Blocked by: ImageGenerationForm implementation
    // Priority: P0
    test("prevents duplicate submissions while generating", async () => {
      const requests: unknown[] = [];
      let resolveResponse!: () => void;
      const responseReady = new Promise<void>((resolve) => {
        resolveResponse = resolve;
      });
      worker.use(
        getPostImagesMockHandler(async ({ request }) => {
          requests.push(await request.json());
          await responseReady;
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
    // ID: IMAGE-GENERATION-FORM-S-007
    // Source: docs/v1/ui/ui.md § 11; docs/v1/ui/components/ImageGenerationForm.md § State model
    // Given: POST /images returns 422 with errors[].field entries
    // When: The response is handled by the form
    // Then: Each returned field error is displayed beside its corresponding input and associated accessibly
    // Error: 422 Unprocessable Entity with field-level errors
    // Blocked by: ImageGenerationForm implementation
    // Priority: P0
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

      await waitFor(() => {
        expect(
          screen.getByRole("textbox", { name: "描画する文字列" }),
        ).toHaveAccessibleErrorMessage("描画する文字列が不正です。");
      });
    });
  });

  describe("API error banners", () => {
    // ID: IMAGE-GENERATION-FORM-S-008
    // Source: docs/v1/ui/ui.md § 12; docs/v1/ui/components/ImageGenerationForm.md § State model
    // Given: POST /images returns an API error with a localized message
    // When: The response is handled by the form
    // Then: The API message is displayed in the form-level alert
    // Blocked by: ImageGenerationForm implementation
    // Priority: P1
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
      expect(alert).toHaveTextContent("リクエストを確認してください。");
    });
  });

  describe("rate-limit retry behavior", () => {
    // ID: IMAGE-GENERATION-FORM-S-009
    // Source: docs/v1/ui/ui.md § 12 Retry-After; docs/v1/ui/components/ImageGenerationForm.md § State model
    // Given: A 429 response includes a Retry-After duration
    // When: The form receives the response and time advances until the duration expires
    // Then: The generate action remains unavailable during the countdown and becomes retryable at zero without changing the API message
    // Error: 429 Too Many Requests with Retry-After
    // Blocked by: ImageGenerationForm implementation
    // Priority: P1
    test("enforces Retry-After before allowing a retry", async () => {
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

    // ID: IMAGE-GENERATION-FORM-S-010
    // Source: docs/v1/ui/ui.md § 12 Retry-After; docs/v1/ui/components/ImageGenerationForm.md § State model
    // Given: A 429 response has no Retry-After header
    // When: The form receives the response
    // Then: The generate action is immediately retryable
    // Error: 429 Too Many Requests without Retry-After
    // Blocked by: ImageGenerationForm implementation
    // Priority: P1
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

  describe("successful generation", () => {
    // ID: IMAGE-GENERATION-FORM-S-011
    // Source: docs/v1/ui/ui.md § 8, § 10; docs/v1/ui/components/ImageGenerationForm.md § State model
    // Given: POST /images returns a successful PNG response with Content-Disposition
    // When: The form handles the successful response
    // Then: The PNG is downloaded automatically with the response filename and no preview is rendered
    // Blocked by: ImageGenerationForm implementation
    // Priority: P0
    test.todo("downloads the generated PNG automatically");
  });
});
