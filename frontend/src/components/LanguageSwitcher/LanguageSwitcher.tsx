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
  options: readonly LanguageOption[];
  onChange: (locale: Locale) => void;
};

const LanguageSwitcher = ({
  locale,
  options,
  onChange,
}: LanguageSwitcherProps) => {
  const selectedLanguage = options.find((option) => option.locale === locale);

  return (
    <DropdownMenu>
      <DropdownMenuTrigger asChild>
        <button type={"button"}>
          {selectedLanguage?.label}
          <ChevronDown aria-hidden={"true"} />
        </button>
      </DropdownMenuTrigger>
      <DropdownMenuContent>
        {options.map((option) => (
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
