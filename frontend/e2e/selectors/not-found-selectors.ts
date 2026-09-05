import type { Locale } from "../../src/types/i18n.ts";

export const notFoundHomeLinkName = (locale: Locale) => {
  switch (locale) {
    case "ja": return /トップページへ戻る/;
    case "en": return /Back to Home/;
  }
};
