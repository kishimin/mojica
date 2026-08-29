import { createContext, type ReactNode, useContext, useState } from "react";

const localeDefinitions = {
  en: null,
  ja: null,
};

type Locale = keyof typeof localeDefinitions;

type I18nContextValue = {
  locale: Locale;
  setLocale: (locale: Locale) => void;
};

type I18nProviderProps = {
  children: ReactNode;
};

const I18nContext = createContext<I18nContextValue | undefined>(undefined);

const isLocale = (value: string): value is Locale =>
  Object.hasOwn(localeDefinitions, value);

const getInitialLocale = (): Locale => {
  const persistedLocale = localStorage.getItem("locale");

  return persistedLocale !== null && isLocale(persistedLocale)
    ? persistedLocale
    : "ja";
};

export const I18nProvider = ({ children }: I18nProviderProps) => {
  const [locale, setLocaleState] = useState(getInitialLocale);

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

export const useI18n = () => {
  const context = useContext(I18nContext);

  if (context === undefined) {
    throw new Error("useI18n must be used within I18nProvider");
  }

  return context;
};
