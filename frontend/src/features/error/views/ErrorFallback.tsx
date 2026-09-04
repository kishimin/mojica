const messages = {
  ja: {
    heading: "エラーが発生しました",
    description:
      "予期しない問題が発生しました。しばらくしてからページを再読み込みしてください。",
    button: "ページを再読み込み",
  },
  en: {
    heading: "An error occurred",
    description: "Something unexpected happened. Please reload the page and try again.",
    button: "Reload page",
  },
} as const;

type SupportedLocale = keyof typeof messages;

const defaultLocale: SupportedLocale = "ja";

const isSupportedLocale = (value: string): value is SupportedLocale =>
  Object.hasOwn(messages, value);

const resolveLocale = (): SupportedLocale => {
  const candidate = navigator.languages[0]?.toLowerCase();
  const language = candidate?.split("-")[0];

  return language && isSupportedLocale(language) ? language : defaultLocale;
};

const ErrorFallback = () => {
  const copy = messages[resolveLocale()];

  return (
    <main>
      <h1>{copy.heading}</h1>
      <p>{copy.description}</p>
      <button type="button" onClick={() => window.location.reload()}>
        {copy.button}
      </button>
    </main>
  );
};

export default ErrorFallback;
