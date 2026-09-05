import type { LocalizedSelector } from "./localized-selector.ts";

/** Stable accessible names used by the error-fallback page object. */
export const errorFallbackSelectors = {
  reloadButton: {
    ja: /ページを再読み込み/,
    en: /Reload page/,
  },
} satisfies Record<"reloadButton", LocalizedSelector>;
