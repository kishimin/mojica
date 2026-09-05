/** Minimal copy kept independent from the provider-backed i18n messages. */
export const errorFallbackMessages = {
  ja: {
    heading: "エラーが発生しました",
    description:
      "予期しない問題が発生しました。しばらくしてからページを再読み込みしてください。",
    button: "ページを再読み込み",
  },
  en: {
    heading: "An error occurred",
    description:
      "Something unexpected happened. Please reload the page and try again.",
    button: "Reload page",
  },
} as const;

export type ErrorFallbackSupportedLocale = keyof typeof errorFallbackMessages;
