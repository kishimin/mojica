import { ChevronDown } from "lucide-react";
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu";
import { localeDefinitions, type Locale } from "@/types/i18n";

type LanguageSwitcherProps = {
  locale: Locale;
  onChange: (locale: Locale) => void;
};

const languageOptions = Object.values(localeDefinitions);

const LanguageSwitcher = ({ locale, onChange }: LanguageSwitcherProps) => {
  const selectedLanguage = localeDefinitions[locale];

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
