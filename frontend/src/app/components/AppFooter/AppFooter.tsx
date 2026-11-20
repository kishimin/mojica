import { Link } from "@tanstack/react-router";
import { useI18n } from "@/hooks/use-i18n";
import type { Locale } from "@/types/i18n";

const contactForms = {
  ja: {
    label: "お問い合わせ",
    url: "https://docs.google.com/forms/d/e/1FAIpQLSdG8CcjnmGBseY6ExWhEidXutkXzyU3VmVrhFfQs9crJeOndQ/viewform",
  },
  en: {
    label: "Contact",
    url: "https://docs.google.com/forms/d/e/1FAIpQLSedj8RXI0wSVnIHYu4jyRi7oyZ9jRFH98QGLyXAHtHC10XqFw/viewform",
  },
} satisfies Record<Locale, { label: string; url: string }>;

const privacyLabel = (locale: Locale) => {
  switch (locale) {
    case "ja":
      return "プライバシーポリシー";
    case "en":
      return "Privacy Policy";
  }
};

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
        href={contactForms[locale].url}
      >
        {contactForms[locale].label}
      </a>
      <Link className={"underline"} to={"/privacy"}>
        {privacyLabel(locale)}
      </Link>
    </footer>
  );
};

export default AppFooter;
