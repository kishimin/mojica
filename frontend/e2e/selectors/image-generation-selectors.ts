import type { LocalizedSelector } from "./localized-selector.ts";
export const imageGenerationSelectorKeys = {
  heading: "heading",
  textLabel: "textLabel",
  foregroundCharacterLabel: "foregroundCharacterLabel",
  backgroundCharacterLabel: "backgroundCharacterLabel",
  submitButton: "submitButton",
} as const;
type ImageGenerationSelectorKey =
  (typeof imageGenerationSelectorKeys)[keyof typeof imageGenerationSelectorKeys];

export const imageGenerationSelectors = {
  heading: {
    ja: /文字で、文字を描く。/,
    en: /Draw letters with letters\./,
  },
  textLabel: {
    ja: /描画する文字列/,
    en: /Text to draw/,
  },
  foregroundCharacterLabel: {
    ja: /^描画に使う文字$/,
    en: /^Character used to render text$/,
  },
  backgroundCharacterLabel: {
    ja: /^敷き詰める文字$/,
    en: /^Background character$/,
  },
  submitButton: {
    ja: /画像を生成する/,
    en: /Generate image/,
  },
} satisfies Record<ImageGenerationSelectorKey, LocalizedSelector>;
