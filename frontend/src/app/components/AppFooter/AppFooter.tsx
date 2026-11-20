import { Link } from "@tanstack/react-router";
import { useI18n } from "@/hooks/use-i18n";
import { appFooterMessages } from "@/i18n/messages";
import type { Locale } from "@/types/i18n";

const contactForms = {
  ja: "https://docs.google.com/forms/d/e/1FAIpQLSdG8CcjnmGBseY6ExWhEidXutkXzyU3VmVrhFfQs9crJeOndQ/viewform",
  en: "https://docs.google.com/forms/d/e/1FAIpQLSedj8RXI0wSVnIHYu4jyRi7oyZ9jRFH98QGLyXAHtHC10XqFw/viewform",
} satisfies Record<Locale, string>;

/** Provides the module's public behavior. */
const AppFooter = () => {
  const { locale } = useI18n();
  const messages = appFooterMessages[locale];

  return (
    <footer
      className={
        "flex min-h-[var(--layout-footer-height)] flex-wrap items-center justify-center gap-4 px-6 py-3 text-center text-sm text-helper-foreground"
      }
    >
      {"© kishimin 2026"}
      <a
        className={"underline"}
        href={contactForms[locale]}
      >
        {messages.contact}
      </a>
      <Link className={"underline"} to={"/privacy"}>
        {messages.privacy}
      </Link>
    </footer>
  );
};

export default AppFooter;
