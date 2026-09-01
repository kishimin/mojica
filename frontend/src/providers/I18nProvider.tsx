import { type ReactNode, useEffect, useRef, useState } from "react";
import { I18nContext } from "../hooks/i18n-context";
import { localeDefinitions, type Locale } from "../types/i18n";

type I18nProviderProps = {
  children: ReactNode;
  initialLocale?: Locale;
};

const isLocale = (value: string): value is Locale =>
  Object.hasOwn(localeDefinitions, value);

const getInitialLocale = (): Locale => {
  try {
    const persistedLocale = localStorage.getItem("locale");

    return persistedLocale !== null && isLocale(persistedLocale)
      ? persistedLocale
      : "ja";
  } catch {
    return "ja";
  }
};

export const I18nProvider = ({
  children,
  initialLocale,
}: I18nProviderProps) => {
  const [locale, setLocaleState] = useState(
    () => initialLocale ?? getInitialLocale(),
  );
  const previousDocumentLanguage = useRef<string | undefined>(undefined);

  useEffect(() => {
    if (previousDocumentLanguage.current === undefined) {
      previousDocumentLanguage.current = document.documentElement.lang;
    }

    document.documentElement.lang = locale;

    return () => {
      if (previousDocumentLanguage.current !== undefined) {
        document.documentElement.lang = previousDocumentLanguage.current;
      }
    };
  }, [locale]);

  const setLocale = (nextLocale: Locale) => {
    localStorage.setItem("locale", nextLocale);
    setLocaleState(nextLocale);
  };

  return (
    <I18nContext.Provider value={{ locale, setLocale }}>
      {children}
    </I18nContext.Provider>
  );
};
