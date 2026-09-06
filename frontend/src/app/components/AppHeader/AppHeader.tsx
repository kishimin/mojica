import LanguageSwitcher from "@/components/LanguageSwitcher/LanguageSwitcher";
import Logo from "@/components/Logo/Logo";
import { useI18n } from "@/hooks/use-i18n";

/** Provides the module's public behavior. */
const AppHeader = () => {
  const { locale, setLocale } = useI18n();

  return (
    <header
      className={
        "flex h-[var(--layout-header-height)] items-center justify-between px-[var(--layout-header-inline-padding)]"
      }
    >
      <Logo />
      <LanguageSwitcher locale={locale} onChange={setLocale} />
    </header>
  );
};

export default AppHeader;
