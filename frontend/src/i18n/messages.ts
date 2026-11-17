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
  mode: string;
  textMode: string;
  imageMode: string;
  imageFile: string;
  imageHelp: string;
  imageRequired: string;
  imageOutputTooLarge: string;
  imageForeground: string;
  imageForegroundColor: string;
  imageForegroundColorPicker: string;
  imageBackground: string;
  imageBackgroundColor: string;
  imageBackgroundColorPicker: string;
  text: string;
  foregroundCharacter: string;
  foregroundColor: string;
  foregroundColorPicker: string;
  backgroundCharacter: string;
  backgroundColor: string;
  backgroundColorPicker: string;
  previewPlaceholder: string;
  previewAlt: string;
  download: string;
};

export const imageGenerationFormMessages = {
  ja: {
    mode: "作成方法",
    textMode: "文字を文字で描く",
    imageMode: "画像を文字で描く",
    imageFile: "元画像",
    imageHelp: "PNG、JPEG、WebP形式で、2 MiB以下、縦横204ピクセル以下の静止画像を選んでください。",
    imageRequired: "元画像を選択してください。",
    imageOutputTooLarge: "元画像の縦横のサイズを小さくして、もう一度お試しください。",
    imageForeground: "絵柄に使う文字",
    imageForegroundColor: "絵柄の文字色",
    imageForegroundColorPicker: "絵柄の文字色を選択",
    imageBackground: "背景に使う文字",
    imageBackgroundColor: "背景の文字色",
    imageBackgroundColorPicker: "背景の文字色を選択",
    text: "描画する文字列",
    foregroundCharacter: "描画に使う文字",
    foregroundColor: "描画に使う文字の色",
    foregroundColorPicker: "描画に使う文字の色を選択",
    backgroundCharacter: "敷き詰める文字",
    backgroundColor: "敷き詰める文字の色",
    backgroundColorPicker: "敷き詰める文字の色を選択",
    previewPlaceholder: "生成するとここに表示されます",
    previewAlt: "生成した画像",
    download: "PNGをダウンロード",
  },
  en: {
    mode: "Creation method",
    textMode: "Draw text with text",
    imageMode: "Draw images with text",
    imageFile: "Source image",
    imageHelp: "Choose a non-animated PNG, JPEG, or WebP up to 2 MiB and 204 pixels per side.",
    imageRequired: "Choose a source image.",
    imageOutputTooLarge: "Reduce the source image dimensions and try again.",
    imageForeground: "Pattern characters",
    imageForegroundColor: "Pattern character color",
    imageForegroundColorPicker: "Choose pattern character color",
    imageBackground: "Background characters",
    imageBackgroundColor: "Background character color",
    imageBackgroundColorPicker: "Choose background character color",
    text: "Text to render",
    foregroundCharacter: "Character used to render text",
    foregroundColor: "Foreground character color",
    foregroundColorPicker: "Choose foreground character color",
    backgroundCharacter: "Background character",
    backgroundColor: "Background character color",
    backgroundColorPicker: "Choose background character color",
    previewPlaceholder: "Your generated image will appear here",
    previewAlt: "Generated image",
    download: "Download PNG",
  },
} satisfies Record<Locale, ImageGenerationFormMessages>;

type ImageGenerationScreenMessages = {
  heading: string;
  description: string;
  textHelper: string;
  characterHelper: string;
  downloadHelper: string;
};

export const imageGenerationScreenMessages = {
  ja: {
    heading: "文字で描く。",
    description:
      "好きな文字と2つの色を組み合わせて、文字アート画像を生成します。",
    textHelper: "1〜64文字",
    characterHelper: "1〜128文字",
    downloadHelper: "生成した画像を確認してからダウンロードできます。",
  },
  en: {
    heading: "Draw with text.",
    description:
      "Combine characters and two colors to create text art.",
    textHelper: "1–64 characters",
    characterHelper: "1–128 characters",
    downloadHelper: "Preview the generated image before downloading it.",
  },
} satisfies Record<Locale, ImageGenerationScreenMessages>;

type NotFoundViewMessages = {
  status: string;
  heading: string;
  description: string;
  homeLink: string;
};

export const notFoundViewMessages = {
  ja: {
    status: "404",
    heading: "ページが見つかりません",
    description: "URLが正しいか確認するか、トップページへ戻ってください。",
    homeLink: "トップページへ戻る",
  },
  en: {
    status: "404",
    heading: "Page not found",
    description: "Please check the URL or return to the homepage.",
    homeLink: "Back to Home",
  },
} satisfies Record<Locale, NotFoundViewMessages>;

export const imageGenerationValidationMessages: Record<
  Locale,
  Record<string, string>
> = {
  ja: {
    "foregroundColor.invalid":
      "描画に使う文字の色をHEXカラー形式（#RRGGBB）で指定してください。",
    "backgroundColor.invalid":
      "敷き詰める文字の色をHEXカラー形式（#RRGGBB）で指定してください。",
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
    "foregroundColor.invalid":
      "Enter the rendering character color in HEX format (#RRGGBB).",
    "backgroundColor.invalid":
      "Enter the background character color in HEX format (#RRGGBB).",
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
