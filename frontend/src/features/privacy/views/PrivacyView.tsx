import { useI18n } from "@/hooks/use-i18n";

/** Displays the service privacy policy. */
const PrivacyView = () => {
  const { locale } = useI18n();

  return (
    <main className={"mx-auto w-full max-w-3xl flex-1 space-y-6 px-6 py-12"}>
      {locale === "ja" ? (
        <>
          <h1 className={"text-2xl font-semibold"}>{"プライバシーポリシー"}</h1>
          <p>{"mojicaの運営者（kishimin）は、サービスの利用に伴う情報を次のように取り扱います。"}</p>
          <section className={"space-y-2"}>
            <h2 className={"text-xl font-semibold"}>{"お問い合わせ"}</h2>
            <p>{"お問い合わせにはGoogleフォームを利用します。フォームに入力されたお名前（ニックネームを含む）やお問い合わせ内容は、お問い合わせへの対応とサービスの改善に利用します。個人情報を含む内容は、必要な範囲でのみご入力ください。"}</p>
            <p>{"Googleフォームで送信された情報の取り扱いについては、"}<a className={"underline"} href={"https://policies.google.com/privacy?hl=ja"}>{"Googleのプライバシーポリシー"}</a>{"もご確認ください。"}</p>
          </section>
          <section className={"space-y-2"}>
            <h2 className={"text-xl font-semibold"}>{"サービスの利用に伴う情報"}</h2>
            <p>{"画像の生成時には、入力された文字や設定が生成処理のためにサーバーへ送信されます。サービスの提供に用いる事業者により、アクセスに伴う情報が処理される場合があります。"}</p>
            <p>{"本サイトでは、利用状況の把握とサービスの改善のため、Googleタグマネージャーを通じてGoogleアナリティクスを使用しています。GoogleアナリティクスはCookieなどを利用して利用状況を収集します。ブラウザの設定からCookieの保存を制限または削除できます。詳細は"}<a className={"underline"} href={"https://policies.google.com/technologies/partner-sites?hl=ja"}>{"Googleの説明"}</a>{"をご確認ください。"}</p>
          </section>
          <section className={"space-y-2"}>
            <h2 className={"text-xl font-semibold"}>{"お問い合わせ先"}</h2>
            <p>{"情報の取り扱いに関するご質問は、フッターの「お問い合わせ」からお送りください。"}</p>
          </section>
        </>
      ) : (
        <>
          <h1 className={"text-2xl font-semibold"}>{"Privacy Policy"}</h1>
          <p>{"The operator of mojica, kishimin, handles information associated with the service as described below."}</p>
          <section className={"space-y-2"}>
            <h2 className={"text-xl font-semibold"}>{"Contact"}</h2>
            <p>{"We use Google Forms for inquiries. We use the name you provide, including a nickname, and the contents of your inquiry to respond and improve the service. Please only include personal information when necessary."}</p>
            <p>{"For information about Google Forms, see the "}<a className={"underline"} href={"https://policies.google.com/privacy?hl=en"}>{"Google Privacy Policy"}</a>{"."}</p>
          </section>
          <section className={"space-y-2"}>
            <h2 className={"text-xl font-semibold"}>{"Information used by the service"}</h2>
            <p>{"When you generate an image, your text and settings are sent to a server for processing. Service providers may process information associated with your access to the service."}</p>
            <p>{"This site uses Google Analytics through Google Tag Manager to understand usage and improve the service. Google Analytics uses cookies and similar technologies to collect usage information. You can restrict or delete cookies through your browser settings. See "}<a className={"underline"} href={"https://policies.google.com/technologies/partner-sites?hl=en"}>{"Google's explanation"}</a>{" for details."}</p>
          </section>
          <section className={"space-y-2"}>
            <h2 className={"text-xl font-semibold"}>{"Contact us"}</h2>
            <p>{"For questions about the handling of information, use the Contact link in the footer."}</p>
          </section>
        </>
      )}
    </main>
  );
};

export default PrivacyView;
