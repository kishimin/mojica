import { type ReactNode, useEffect, useState } from "react";
import { I18nContext } from "../hooks/i18n-context";
import { localeDefinitions, type Locale } from "../types/i18n";

type I18nProviderProps = {
  children: ReactNode;
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

export const I18nProvider = ({ children }: I18nProviderProps) => {
  const [locale, setLocaleState] = useState(getInitialLocale);

  useEffect(() => {
    document.documentElement.lang = locale;
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
