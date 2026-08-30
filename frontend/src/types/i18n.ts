export const localeDefinitions = {
  ja: null,
  en: null,
};

export type Locale = keyof typeof localeDefinitions;

export type I18nContextType = {
  locale: Locale;
  setLocale: (locale: Locale) => void;
};
