import { useI18n } from "@/hooks/use-i18n";

/** Provides the module's public behavior. */
const AppFooter = () => {
  const { locale } = useI18n();

  return (
    <footer
      className={
        "flex h-[var(--layout-footer-height)] items-center justify-center gap-4 px-6 text-center text-sm text-helper-foreground"
      }
    >
      {"© kishimin 2026"}
      <a
        className={"underline"}
        href={
          "https://docs.google.com/forms/d/e/1FAIpQLSdG8CcjnmGBseY6ExWhEidXutkXzyU3VmVrhFfQs9crJeOndQ/viewform"
        }
      >
        {locale === "ja" ? "お問い合わせ" : "Contact"}
      </a>
    </footer>
  );
};

export default AppFooter;
