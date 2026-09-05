import type { Locale } from "../../src/types/i18n.ts";

/** Stable accessible names used by the error-fallback page object. */
export const errorFallbackReloadButtonName = (locale: Locale) => {
  switch (locale) {
    case "ja":
      return /ページを再読み込み/;
    case "en":
      return /Reload page/;
  }
};
