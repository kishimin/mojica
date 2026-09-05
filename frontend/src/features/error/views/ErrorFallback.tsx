import { errorFallbackMessages } from "../i18n/error-fallback-messages";
import { resolveErrorFallbackLocale } from "../i18n/resolve-error-fallback-locale";
import { I18nProvider } from "@/providers/I18nProvider";

type ErrorFallbackContentProps = {
  locale: keyof typeof errorFallbackMessages;
};

const ErrorFallbackContent = ({ locale }: ErrorFallbackContentProps) => {
  const copy = errorFallbackMessages[locale];

  return (
    <main
      className={
        "flex min-h-screen items-center justify-center overflow-clip bg-background px-4"
      }
    >
      <div className={"flex w-full flex-col items-center gap-6 text-center"}>
        <h1 className={"text-[2rem] font-bold leading-normal text-foreground"}>
          {copy.heading}
        </h1>
        <p className={"text-sm font-normal text-muted-foreground"}>
          {copy.description}
        </p>
        <button
          type={"button"}
          onClick={() => window.location.reload()}
          className={
            "rounded-md bg-primary px-4 py-2 text-primary-foreground hover:bg-primary/80"
          }
        >
          {copy.button}
        </button>
      </div>
    </main>
  );
};

const ErrorFallback = () => {
  const locale = resolveErrorFallbackLocale();

  return (
    <I18nProvider initialLocale={locale}>
      <ErrorFallbackContent locale={locale} />
    </I18nProvider>
  );
};

export default ErrorFallback;
