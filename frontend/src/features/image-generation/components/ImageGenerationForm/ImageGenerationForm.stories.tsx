import type { Meta, StoryObj } from "@storybook/react-vite";
import { HttpResponse, http } from "msw";
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
};

export const ValidationError: Story = {
  args: { locale: "ja" },
};

export const TextTooLong: Story = {
  args: { locale: "ja" },
};

export const RenderingCharacterTooLong: Story = {
  args: { locale: "ja" },
};

export const BackgroundCharacterTooLong: Story = {
  args: { locale: "ja" },
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
};

export const Success: Story = {
  args: { locale: "ja" },
  parameters: {
    msw: { handlers: [getPostImagesMockHandler(new ArrayBuffer(1))] },
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
};

export const InternalServerError: Story = {
  args: { locale: "ja" },
  parameters: {
    msw: {
      handlers: [apiErrorHandler("INTERNAL_SERVER_ERROR", "内部エラー", 500)],
    },
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
};
