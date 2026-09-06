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
  /** Currently selected locale. */
  locale: Locale;
  /** Called after the user selects another locale. */
  onChange: (locale: Locale) => void;
};

/** Provides the module's public behavior. */
const LanguageSwitcher = ({ locale, onChange }: LanguageSwitcherProps) => {
  const selectedLanguage = languageOptions.find(
    (option) => option.locale === locale,
  );

  return (
    <DropdownMenu>
      <DropdownMenuTrigger
        className={
          "inline-flex h-10 w-[142px] items-center justify-between rounded-lg border border-border bg-surface px-3.5 text-sm font-medium"
        }
      >
        {selectedLanguage?.label}
        <ChevronDown aria-hidden={"true"} className={"size-4"} />
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
