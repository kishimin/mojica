import {
  errorFallbackMessages,
  type ErrorFallbackSupportedLocale,
} from "../i18n/error-fallback-messages";

const defaultLocale: ErrorFallbackSupportedLocale = "ja";

const isSupportedLocale = (
  value: string,
): value is ErrorFallbackSupportedLocale =>
  Object.hasOwn(errorFallbackMessages, value);

const readStoredLocale = () => {
  try {
    return localStorage.getItem("locale");
  } catch {
    return null;
  }
};

const resolveLocale = (): ErrorFallbackSupportedLocale => {
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
  const copy = errorFallbackMessages[resolveLocale()];

  return (
    <main>
      <h1>{copy.heading}</h1>
      <p>{copy.description}</p>
      <button type={"button"} onClick={() => window.location.reload()}>
        {copy.button}
      </button>
    </main>
  );
};

export default ErrorFallback;
