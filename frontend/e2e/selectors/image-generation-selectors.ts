import type { ImageType } from "../../src/types/image-type.ts";
import type { LocalizedSelector } from "./localized-selector.ts";
export const imageGenerationSelectorKeys = {
  heading: "heading",
  textLabel: "textLabel",
  foregroundCharacterLabel: "foregroundCharacterLabel",
  foregroundColorLabel: "foregroundColorLabel",
  backgroundCharacterLabel: "backgroundCharacterLabel",
  backgroundColorLabel: "backgroundColorLabel",
  submitButton: "submitButton",
  downloadButton: "downloadButton",
  preview: "preview",
  imageTypeLabel: "imageTypeLabel",
  imageMode: "imageMode",
  sourceImage: "sourceImage",
  patternCharacters: "patternCharacters",
  imageBackgroundCharacters: "imageBackgroundCharacters",
} as const;
type ImageGenerationSelectorKey =
  (typeof imageGenerationSelectorKeys)[keyof typeof imageGenerationSelectorKeys];

export const imageGenerationSelectors = {
  heading: {
    ja: /文字で描く。/,
    en: /Draw with text\./,
  },
  textLabel: {
    ja: /描画する文字列/,
    en: /Text to render/,
  },
  foregroundCharacterLabel: {
    ja: /^描画に使う文字$/,
    en: /^Character used to render text$/,
  },
  foregroundColorLabel: {
    ja: /^描画に使う文字の色$/,
    en: /^Foreground character color$/,
  },
  backgroundCharacterLabel: {
    ja: /^敷き詰める文字$/,
    en: /^Background character$/,
  },
  backgroundColorLabel: {
    ja: /^敷き詰める文字の色$/,
    en: /^Background character color$/,
  },
  submitButton: {
    ja: /画像を生成する/,
    en: /Generate image/,
  },
  downloadButton: {
    ja: /PNGをダウンロード/,
    en: /Download PNG/,
  },
  preview: {
    ja: /生成した画像/,
    en: /Generated image/,
  },
  imageTypeLabel: {
    ja: /画像タイプ/,
    en: /Image type/,
  },
  imageMode: { ja: /画像を文字で描く/, en: /Draw images with text/ },
  sourceImage: { ja: /元画像/, en: /Source image/ },
  patternCharacters: { ja: /^絵柄に使う文字$/, en: /^Pattern characters$/ },
  imageBackgroundCharacters: { ja: /^背景に使う文字$/, en: /^Background characters$/ },
} satisfies Record<ImageGenerationSelectorKey, LocalizedSelector>;

export const imageTypeOptionSelectors = {
  standard: { ja: /標準画像/, en: /Standard image/ },
  "x-background": { ja: /X背景画像/, en: /X background image/ },
  "x-icon": { ja: /Xアイコン画像/, en: /X icon image/ },
} satisfies Record<ImageType, LocalizedSelector>;
