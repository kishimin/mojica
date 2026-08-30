import type { Locale } from "@/types/i18n";

type LanguageOption = {
  locale: Locale;
  label: string;
};

export const languageOptions = [
  { locale: "ja", label: "日本語" },
  { locale: "en", label: "English" },
] as const satisfies readonly LanguageOption[];
