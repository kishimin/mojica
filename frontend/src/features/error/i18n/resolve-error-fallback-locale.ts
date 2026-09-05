import {
  errorFallbackMessages,
  type ErrorFallbackSupportedLocale,
} from "./error-fallback-messages";

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

/** Resolves the fallback locale without depending on the i18n provider. */
export const resolveErrorFallbackLocale = (): ErrorFallbackSupportedLocale => {
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
