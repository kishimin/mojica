import { ChevronDown } from "lucide-react";
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu";
import type { Locale } from "@/types/i18n";

type LanguageOption = {
  locale: Locale;
  label: string;
};

type LanguageSwitcherProps = {
  locale: Locale;
  onChange: (locale: Locale) => void;
};

const languageOptions = [
  { locale: "ja", label: "日本語" },
  { locale: "en", label: "English" },
] as const satisfies readonly LanguageOption[];

const LanguageSwitcher = ({ locale, onChange }: LanguageSwitcherProps) => {
  const selectedLanguage = languageOptions.find(
    (option) => option.locale === locale,
  );

  return (
    <DropdownMenu>
      <DropdownMenuTrigger asChild>
        <button type={"button"}>
          {selectedLanguage?.label}
          <ChevronDown aria-hidden={"true"} />
        </button>
      </DropdownMenuTrigger>
      <DropdownMenuContent>
        {languageOptions.map((option) => (
          <DropdownMenuItem
            key={option.locale}
            onSelect={() => onChange(option.locale)}
          >
            {option.label}
          </DropdownMenuItem>
        ))}
      </DropdownMenuContent>
    </DropdownMenu>
  );
};

export default LanguageSwitcher;
