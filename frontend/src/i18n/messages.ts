import type { Locale } from "@/types/i18n";

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
} satisfies Record<Locale, object>;
