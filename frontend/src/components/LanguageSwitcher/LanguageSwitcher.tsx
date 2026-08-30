import { ChevronDown } from "lucide-react";
import { languageOptions } from "./language-options";
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu";
import type { Locale } from "@/types/i18n";

type LanguageSwitcherProps = {
  locale: Locale;
  onChange: (locale: Locale) => void;
};

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
