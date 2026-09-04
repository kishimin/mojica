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

const readStoredLocale = () => {
  try {
    return localStorage.getItem("locale");
  } catch {
    return null;
  }
};

const resolveLocale = (): SupportedLocale => {
  const candidates = [readStoredLocale(), ...navigator.languages];

  for (const candidate of candidates) {
    const normalized = candidate?.toLowerCase();
    const language = normalized?.split("-")[0];

    if (language && isSupportedLocale(language)) {
      return language;
    }
  }

  return defaultLocale;
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
