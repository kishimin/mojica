/** Stable accessible names used by the image-generation page object. */
export const imageGenerationSelectors = {
  heading: /文字で、文字を描く。/,
  text: /描画する文字列/,
  foregroundCharacter: /描画に使う文字/,
  backgroundCharacter: /敷き詰める文字/,
  submit: /画像を生成する/,
} as const;
