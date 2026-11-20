import { useI18n } from "@/hooks/use-i18n";

/** Displays the privacy policy. */
const PrivacyView = () => {
  const { locale } = useI18n();
  return (
    <main>
      <h1>{locale === "ja" ? "プライバシーポリシー" : "Privacy Policy"}</h1>
    </main>
  );
};

export default PrivacyView;
