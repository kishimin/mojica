import { Link } from "@tanstack/react-router";
import { useI18n } from "@/hooks/use-i18n";

/** Displays the privacy policy. */
const PrivacyView = () => {
  const { locale } = useI18n();
  switch (locale) {
    case "ja":
      return (
        <main className={"mx-auto w-full max-w-3xl flex-1 space-y-6 px-6 py-12"}>
          <h1 className={"text-2xl font-semibold"}>{"プライバシーポリシー"}</h1>
          <h2 className={"text-xl font-semibold"}>{"お問い合わせ"}</h2>
          <p>{"お問い合わせにはGoogleフォームを利用します。フォームに入力されたお名前（ニックネームを含む）やお問い合わせ内容は、お問い合わせへの対応とサービスの改善に利用します。"}</p>
          <p><a className={"underline"} href={"https://policies.google.com/privacy?hl=ja"}>{"Googleのプライバシーポリシー"}</a></p>
          <h2 className={"text-xl font-semibold"}>{"Google アナリティクスによるアクセス解析"}</h2>
          <p>{"本サイトでは、利用状況の把握とサービスの改善のため、Googleタグマネージャーを通じてGoogleアナリティクスを使用しています。"}</p>
          <p>{"GoogleアナリティクスはCookieなどを利用して利用状況を収集します。ブラウザの設定からCookieの保存を制限または削除できます。"}</p>
          <h2 className={"text-xl font-semibold"}>{"画像の生成"}</h2>
          <p>{"画像の生成時には、入力された文字や設定が生成処理のためにサーバーへ送信されます。"}</p>
          <Link className={"underline"} to={"/"}>{"トップページへ戻る"}</Link>
        </main>
      );
    case "en":
      return (
        <main className={"mx-auto w-full max-w-3xl flex-1 space-y-6 px-6 py-12"}>
          <h1 className={"text-2xl font-semibold"}>{"Privacy Policy"}</h1>
          <h2 className={"text-xl font-semibold"}>{"Inquiries"}</h2>
          <p>{"We use Google Forms for inquiries. We use the name you provide, including a nickname, and your inquiry to respond and improve the service."}</p>
          <p><a className={"underline"} href={"https://policies.google.com/privacy?hl=en"}>{"Google Privacy Policy"}</a></p>
          <h2 className={"text-xl font-semibold"}>{"Google Analytics"}</h2>
          <p>{"This site uses Google Analytics through Google Tag Manager to understand usage and improve the service."}</p>
          <p>{"Google Analytics uses cookies to collect usage information. You can restrict or delete cookies through your browser settings."}</p>
          <h2 className={"text-xl font-semibold"}>{"Image generation"}</h2>
          <p>{"When you generate an image, your text and settings are sent to a server for processing."}</p>
          <Link className={"underline"} to={"/"}>{"Back to home"}</Link>
        </main>
      );
  }
};

export default PrivacyView;
