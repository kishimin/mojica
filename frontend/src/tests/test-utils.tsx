import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import {
  render as testingLibraryRender,
  type RenderOptions,
} from "@testing-library/react";
import userEvent from "@testing-library/user-event";
import type { ReactElement } from "react";
import { vi } from "vitest";
import { I18nContext } from "@/hooks/i18n-context";
import type { Locale } from "@/types/i18n";

/** Provides the module's public behavior. */
export const setup = (ui: ReactElement, options?: RenderOptions) => ({
  user: userEvent.setup(),
  ...testingLibraryRender(ui, options),
});

/** Provides the module's public behavior. */
export const setupWithI18n = (ui: ReactElement, locale: Locale = "ja") =>
  setup(
    <I18nContext.Provider
      value={{ locale, setLocale: vi.fn<(nextLocale: Locale) => void>() }}
    >
      {ui}
    </I18nContext.Provider>,
  );

/** Provides the module's public behavior. */
export const setupWithProviders = (ui: ReactElement, locale: Locale = "ja") =>
  setupWithI18n(
    <QueryClientProvider client={new QueryClient()}>{ui}</QueryClientProvider>,
    locale,
  );
