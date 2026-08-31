import type { Locale } from "@/types/i18n";
import type { ImageType } from "@/types/image-type";

type GenerateButtonMessages = {
  idle: string;
  submitting: string;
  retryable: string;
  cooldown: (seconds: number) => string;
};

export const generateButtonMessages = {
  ja: {
    idle: "画像を生成する",
    submitting: "生成中...",
    retryable: "画像を生成する",
    cooldown: (seconds: number) => `${seconds}秒後に再試行できます`,
  },
  en: {
    idle: "Generate image",
    submitting: "Generating...",
    retryable: "Generate image",
    cooldown: (seconds: number) => `You can retry in ${seconds} seconds`,
  },
} satisfies Record<Locale, GenerateButtonMessages>;

type ImageTypeSelectMessages = {
  label: string;
  options: Record<ImageType, string>;
};

export const imageTypeSelectMessages = {
  ja: {
    label: "画像タイプ",
    options: {
      standard: "標準画像",
      "x-background": "X背景画像",
      "x-icon": "Xアイコン画像",
    },
  },
  en: {
    label: "Image type",
    options: {
      standard: "Standard image",
      "x-background": "X background image",
      "x-icon": "X icon image",
    },
  },
} satisfies Record<Locale, ImageTypeSelectMessages>;
