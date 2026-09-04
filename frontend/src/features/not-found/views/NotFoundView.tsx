import { useI18n } from "@/hooks/use-i18n";
import { notFoundViewMessages } from "@/i18n/messages";

/** Renders the localized 404 page for unknown paths. */
const NotFoundView = () => {
  const { locale } = useI18n();
  const messages = notFoundViewMessages[locale];

  return (
    <main
      className={"flex flex-1 flex-col items-center px-4 py-16 text-center"}
    >
      <div className={"flex max-w-[620px] flex-col items-center gap-4"}>
        <h1 className={"text-6xl font-semibold"}>{messages.status}</h1>
        <h2 className={"text-2xl font-semibold"}>{messages.heading}</h2>
        <p className={"text-sm text-muted-foreground"}>
          {messages.description}
        </p>
        <a
          className={"rounded-md bg-primary px-4 py-2 text-primary-foreground"}
          href={"/"}
        >
          {messages.homeLink}
        </a>
      </div>
    </main>
  );
};

export default NotFoundView;
