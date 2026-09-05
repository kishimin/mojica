import type { Locale } from "../../src/types/i18n.ts";

export const notFoundSelectors = {
  homeLink: {
    ja: /トップページへ戻る/,
    en: /Back to Home/,
  },
} satisfies Record<"homeLink", Record<Locale, RegExp>>;
