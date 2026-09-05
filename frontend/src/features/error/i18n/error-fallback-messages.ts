/** Minimal copy kept independent from the provider-backed i18n messages. */
export const errorFallbackMessages = {
  ja: {
    heading: "エラーが発生しました",
    description:
      "予期しないエラーが発生しました。ページを再読み込みして、もう一度お試しください。",
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
