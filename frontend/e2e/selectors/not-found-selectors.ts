import type { LocalizedSelector } from "./localized-selector.ts";

export const notFoundSelectors = {
  homeLink: {
    ja: /トップページへ戻る/,
    en: /Back to Home/,
  },
} satisfies Record<"homeLink", LocalizedSelector>;
