import LanguageSwitcher from "@/components/LanguageSwitcher/LanguageSwitcher";
import Logo from "@/components/Logo/Logo";
import { useI18n } from "@/hooks/use-i18n";

const AppHeader = () => {
  const { locale, setLocale } = useI18n();

  return (
    <header className={"flex items-center justify-between"}>
      <Logo />
      <LanguageSwitcher locale={locale} onChange={setLocale} />
    </header>
  );
};

export default AppHeader;
