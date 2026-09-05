import type { Locale } from "../../src/types/i18n.ts";

export const imageGenerationHeadingName = (locale: Locale) => {
  switch (locale) {
    case "ja": return /文字で、文字を描く。/;
    case "en": return /Draw letters with letters\./;
  }
};
export const imageGenerationTextLabel = (locale: Locale) => {
  switch (locale) {
    case "ja": return /描画する文字列/;
    case "en": return /Text to draw/;
  }
};
export const imageGenerationForegroundCharacterLabel = (locale: Locale) => {
  switch (locale) {
    case "ja": return /描画に使う文字/;
    case "en": return /Character used to render text/;
  }
};
export const imageGenerationBackgroundCharacterLabel = (locale: Locale) => {
  switch (locale) {
    case "ja": return /敷き詰める文字/;
    case "en": return /Background character/;
  }
};
export const imageGenerationSubmitButtonName = (locale: Locale) => {
  switch (locale) {
    case "ja": return /画像を生成する/;
    case "en": return /Generate image/;
  }
};
