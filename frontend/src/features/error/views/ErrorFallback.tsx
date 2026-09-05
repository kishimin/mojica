import { errorFallbackMessages } from "../i18n/error-fallback-messages";
import { resolveErrorFallbackLocale } from "../i18n/resolve-error-fallback-locale";

const ErrorFallback = () => {
  const copy = errorFallbackMessages[resolveErrorFallbackLocale()];

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
          className={"bg-primary text-primary-foreground hover:bg-primary/80"}
        >
          {copy.button}
        </button>
      </div>
    </main>
  );
};

export default ErrorFallback;
