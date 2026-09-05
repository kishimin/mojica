import {
  errorFallbackMessages,
} from "../i18n/error-fallback-messages";
import { resolveErrorFallbackLocale } from "../i18n/resolve-error-fallback-locale";

const ErrorFallback = () => {
  const copy = errorFallbackMessages[resolveErrorFallbackLocale()];

  return (
    <main>
      <h1>{copy.heading}</h1>
      <p>{copy.description}</p>
      <button type={"button"} onClick={() => window.location.reload()}>
        {copy.button}
      </button>
    </main>
  );
};

export default ErrorFallback;
