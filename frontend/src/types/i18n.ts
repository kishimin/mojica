export const localeDefinitions = {
  en: null,
  ja: null,
};

export type Locale = keyof typeof localeDefinitions;

export type I18nContextType = {
  locale: Locale;
  setLocale: (locale: Locale) => void;
};
