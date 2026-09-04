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

type ImageGenerationFormMessages = {
  text: string;
  foregroundCharacter: string;
  foregroundColor: string;
  foregroundColorPicker: string;
  backgroundCharacter: string;
  backgroundColor: string;
  backgroundColorPicker: string;
};

export const imageGenerationFormMessages = {
  ja: {
    text: "描画する文字列",
    foregroundCharacter: "描画に使う文字",
    foregroundColor: "描画に使う文字の色",
    foregroundColorPicker: "描画に使う文字の色を選択",
    backgroundCharacter: "敷き詰める文字",
    backgroundColor: "敷き詰める文字の色",
    backgroundColorPicker: "敷き詰める文字の色を選択",
  },
  en: {
    text: "Text to render",
    foregroundCharacter: "Character used to render text",
    foregroundColor: "Foreground character color",
    foregroundColorPicker: "Choose foreground character color",
    backgroundCharacter: "Background character",
    backgroundColor: "Background character color",
    backgroundColorPicker: "Choose background character color",
  },
} satisfies Record<Locale, ImageGenerationFormMessages>;

export const imageGenerationValidationMessages: Record<
  Locale,
  Record<string, string>
> = {
  ja: {
    "text.required": "描画する文字列を入力してください。",
    "text.whitespaceOnly": "空白以外の文字を入力してください。",
    "text.maxLength": "描画する文字列は64文字以内で入力してください。",
    "text.controlCharacter":
      "描画する文字列には表示可能な文字を含めてください。",
    "foregroundCharacter.required": "描画に使う文字を入力してください。",
    "foregroundCharacter.maxLength":
      "描画に使う文字は128文字以内で入力してください。",
    "foregroundCharacter.controlCharacter":
      "描画に使う文字には制御文字を含めないでください。",
    "backgroundCharacter.required": "敷き詰める文字を入力してください。",
    "backgroundCharacter.maxLength":
      "敷き詰める文字は128文字以内で入力してください。",
    "backgroundCharacter.controlCharacter":
      "敷き詰める文字には制御文字を含めないでください。",
    "characterCombination.required":
      "描画に使う文字または敷き詰める文字のどちらかに、表示可能な文字を入力してください。",
    "imageType.invalid": "画像タイプを選択してください。",
  },
  en: {
    "text.required": "Enter the text to render.",
    "text.whitespaceOnly": "Enter a character other than whitespace.",
    "text.maxLength": "The text to render must be 64 characters or fewer.",
    "text.controlCharacter":
      "The text to render must contain a visible character.",
    "foregroundCharacter.required": "Enter the character used to render text.",
    "foregroundCharacter.maxLength":
      "The rendering character must be 128 characters or fewer.",
    "foregroundCharacter.controlCharacter":
      "The rendering character must not contain control characters.",
    "backgroundCharacter.required": "Enter the background character.",
    "backgroundCharacter.maxLength":
      "The background character must be 128 characters or fewer.",
    "backgroundCharacter.controlCharacter":
      "The background character must not contain control characters.",
    "characterCombination.required":
      "Enter a visible character in either character field.",
    "imageType.invalid": "Select an image type.",
  },
} satisfies Record<Locale, Record<string, string>>;

export type ImageGenerationErrorMessages = Record<
  | "requestError"
  | "requestLimit"
  | "serverError"
  | "imageGenerationServiceError"
  | "timeout"
  | "fallback",
  string
>;

export const imageGenerationErrorMessages = {
  ja: {
    requestError: "リクエストエラー",
    requestLimit: "リクエスト制限",
    serverError: "サーバーエラー",
    imageGenerationServiceError: "画像生成サービスエラー",
    timeout: "タイムアウト",
    fallback: "画像生成エラー",
  },
  en: {
    requestError: "Request error",
    requestLimit: "Request limit",
    serverError: "Server error",
    imageGenerationServiceError: "Image-generation service error",
    timeout: "Timeout",
    fallback: "Image-generation error",
  },
} satisfies Record<Locale, ImageGenerationErrorMessages>;
