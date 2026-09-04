import type { Meta, StoryObj } from "@storybook/react-vite";
import { HttpResponse, http } from "msw";
import { expect, userEvent, within } from "storybook/test";
import ImageGenerationForm from "./ImageGenerationForm";
import { getPostImagesMockHandler } from "@/api/endpoints/image/image.msw";
import { I18nProvider } from "@/providers/I18nProvider";

const meta = {
  title: "Features/Image Generation/ImageGenerationForm",
  component: ImageGenerationForm,
  parameters: {
    docs: {
      description: {
        component:
          "Image-generation form with localized fields, validation feedback, API error handling, and PNG download behavior.",
      },
    },
  },
  render: (args) => (
    <I18nProvider initialLocale={args.locale}>
      <ImageGenerationForm {...args} />
    </I18nProvider>
  ),
} satisfies Meta<typeof ImageGenerationForm>;

export default meta;

type Story = StoryObj<typeof meta>;

const fillRequiredFields = async (canvasElement: HTMLElement) => {
  const canvas = within(canvasElement);

  await userEvent.type(
    canvas.getByRole("textbox", { name: "描画する文字列" }),
    "KA",
  );
  await userEvent.type(
    canvas.getByRole("textbox", { name: "描画に使う文字" }),
    "🌻",
  );
  await userEvent.type(
    canvas.getByRole("textbox", { name: "敷き詰める文字" }),
    "☀",
  );
};

const submitForm = async (canvasElement: HTMLElement) => {
  const canvas = within(canvasElement);
  await userEvent.click(canvas.getByRole("button", { name: "画像を生成する" }));
};

const apiErrorHandler = (code: string, message: string, status: number) =>
  http.post("*/images", () => HttpResponse.json({ code, message }, { status }));

export const Default: Story = {
  args: {
    locale: "ja",
  },
};

export const English: Story = {
  args: {
    locale: "en",
  },
};

export const Filled: Story = {
  args: { locale: "ja" },
  play: async ({ canvasElement }) => {
    await fillRequiredFields(canvasElement);
  },
};

export const ValidationError: Story = {
  args: { locale: "ja" },
  play: async ({ canvasElement }) => {
    const canvas = within(canvasElement);
    await submitForm(canvasElement);
    await expect(
      canvas.getByRole("textbox", { name: "描画する文字列" }),
    ).toHaveAccessibleErrorMessage("描画する文字列を入力してください。");
  },
};

export const TextTooLong: Story = {
  args: { locale: "ja" },
  play: async ({ canvasElement }) => {
    const canvas = within(canvasElement);
    const textField = canvas.getByRole("textbox", {
      name: "描画する文字列",
    });

    await userEvent.type(textField, "a".repeat(65));
    await expect(textField).toHaveAccessibleErrorMessage(
      "描画する文字列は64文字以内で入力してください。",
    );
  },
};

export const RenderingCharacterTooLong: Story = {
  args: { locale: "ja" },
  play: async ({ canvasElement }) => {
    const canvas = within(canvasElement);
    const characterField = canvas.getByRole("textbox", {
      name: "描画に使う文字",
    });

    await userEvent.type(characterField, "a".repeat(129));
    await expect(characterField).toHaveAccessibleErrorMessage(
      "描画に使う文字は128文字以内で入力してください。",
    );
  },
};

export const Submitting: Story = {
  args: { locale: "ja" },
  parameters: {
    msw: {
      handlers: [
        getPostImagesMockHandler(() => new Promise<ArrayBuffer>(() => {})),
      ],
    },
  },
  play: async ({ canvasElement }) => {
    await fillRequiredFields(canvasElement);
    await submitForm(canvasElement);
    await expect(
      within(canvasElement).getByRole("button", { name: "生成中..." }),
    ).toBeDisabled();
  },
};

export const Success: Story = {
  args: { locale: "ja" },
  parameters: {
    msw: { handlers: [getPostImagesMockHandler(new ArrayBuffer(1))] },
  },
  play: async ({ canvasElement }) => {
    await fillRequiredFields(canvasElement);
    await submitForm(canvasElement);
  },
};

export const BadRequest: Story = {
  args: { locale: "ja" },
  parameters: {
    msw: {
      handlers: [
        apiErrorHandler("BAD_REQUEST", "入力内容を確認してください。", 400),
      ],
    },
  },
  play: async ({ canvasElement }) => {
    await fillRequiredFields(canvasElement);
    await submitForm(canvasElement);
    await expect(
      within(canvasElement).findByRole("alert"),
    ).resolves.toHaveTextContent("リクエストエラー");
  },
};

export const RateLimitExceeded: Story = {
  args: { locale: "ja" },
  parameters: {
    msw: {
      handlers: [
        http.post("*/images", () =>
          HttpResponse.json(
            {
              code: "RATE_LIMIT_EXCEEDED",
              message: "しばらく待ってください。",
            },
            { status: 429, headers: { "Retry-After": "5" } },
          ),
        ),
      ],
    },
  },
  play: async ({ canvasElement }) => {
    await fillRequiredFields(canvasElement);
    await submitForm(canvasElement);
    await expect(
      within(canvasElement).findByRole("alert"),
    ).resolves.toHaveTextContent("リクエスト制限");
  },
};

export const InternalServerError: Story = {
  args: { locale: "ja" },
  parameters: {
    msw: {
      handlers: [apiErrorHandler("INTERNAL_SERVER_ERROR", "内部エラー", 500)],
    },
  },
  play: async ({ canvasElement }) => {
    await fillRequiredFields(canvasElement);
    await submitForm(canvasElement);
    await expect(
      within(canvasElement).findByRole("alert"),
    ).resolves.toHaveTextContent("サーバーエラー");
  },
};

export const ImageGenerationServiceError: Story = {
  args: { locale: "ja" },
  parameters: {
    msw: {
      handlers: [
        apiErrorHandler(
          "IMAGE_GENERATION_FAILED",
          "画像生成に失敗しました。",
          502,
        ),
      ],
    },
  },
  play: async ({ canvasElement }) => {
    await fillRequiredFields(canvasElement);
    await submitForm(canvasElement);
    await expect(
      within(canvasElement).findByRole("alert"),
    ).resolves.toHaveTextContent("画像生成サービスエラー");
  },
};

export const Timeout: Story = {
  args: { locale: "ja" },
  parameters: {
    msw: {
      handlers: [
        apiErrorHandler(
          "IMAGE_GENERATION_TIMEOUT",
          "タイムアウトしました。",
          504,
        ),
      ],
    },
  },
  play: async ({ canvasElement }) => {
    await fillRequiredFields(canvasElement);
    await submitForm(canvasElement);
    await expect(
      within(canvasElement).findByRole("alert"),
    ).resolves.toHaveTextContent("タイムアウト");
  },
};
