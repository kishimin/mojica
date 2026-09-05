import type { Locale } from "../../src/types/i18n.ts";

type LocalizedSelector = Record<Locale, RegExp>;

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
    ja: /描画に使う文字/,
    en: /Character used to render text/,
  },
  backgroundCharacterLabel: {
    ja: /敷き詰める文字/,
    en: /Background character/,
  },
  submitButton: {
    ja: /画像を生成する/,
    en: /Generate image/,
  },
} satisfies Record<
  | "heading"
  | "textLabel"
  | "foregroundCharacterLabel"
  | "backgroundCharacterLabel"
  | "submitButton",
  LocalizedSelector
>;
