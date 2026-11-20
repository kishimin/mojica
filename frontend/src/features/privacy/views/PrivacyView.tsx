import { useI18n } from "@/hooks/use-i18n";

/** Displays the privacy policy. */
const PrivacyView = () => {
  const { locale } = useI18n();
  return (
    <main>
      <h1>{locale === "ja" ? "プライバシーポリシー" : "Privacy Policy"}</h1>
      {locale === "ja" && <p>{"お問い合わせにはGoogleフォームを利用します。フォームに入力されたお名前（ニックネームを含む）やお問い合わせ内容は、お問い合わせへの対応とサービスの改善に利用します。"}</p>}
      {locale === "ja" && <p>{"GoogleアナリティクスはCookieなどを利用して利用状況を収集します。ブラウザの設定からCookieの保存を制限または削除できます。"}</p>}
      {locale === "ja" && <p>{"画像の生成時には、入力された文字や設定が生成処理のためにサーバーへ送信されます。"}</p>}
      {locale === "en" && <p>{"We use Google Forms for inquiries. We use the name you provide, including a nickname, and your inquiry to respond and improve the service."}</p>}
      {locale === "en" && <p>{"Google Analytics uses cookies to collect usage information. You can restrict or delete cookies through your browser settings."}</p>}
      {locale === "en" && <p>{"When you generate an image, your text and settings are sent to a server for processing."}</p>}
    </main>
  );
};

export default PrivacyView;
