import { useI18n } from "@/hooks/use-i18n";

/** Displays the privacy policy. */
const PrivacyView = () => {
  const { locale } = useI18n();
  return (
    <main>
      <h1>{locale === "ja" ? "プライバシーポリシー" : "Privacy Policy"}</h1>
      {locale === "ja" && <p>{"お問い合わせにはGoogleフォームを利用します。フォームに入力されたお名前（ニックネームを含む）やお問い合わせ内容は、お問い合わせへの対応とサービスの改善に利用します。"}</p>}
    </main>
  );
};

export default PrivacyView;
