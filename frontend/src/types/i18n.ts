export const localeDefinitions = {
  ja: { locale: "ja", label: "日本語" },
  en: { locale: "en", label: "English" },
} as const;

export type Locale = keyof typeof localeDefinitions;

export type I18nContextType = {
  locale: Locale;
  setLocale: (locale: Locale) => void;
};
