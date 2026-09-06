import { QueryClientProvider } from "@tanstack/react-query";
import type { ReactNode } from "react";
import { queryClient } from "../../lib/react-query";
import { I18nProvider } from "../../providers/I18nProvider";
import ErrorBoundary from "./ErrorBoundary";

type AppProvidersProps = {
  children: ReactNode;
};

/** Provides the module's public behavior. */
export const AppProviders = ({ children }: AppProvidersProps) => {
  return (
    <ErrorBoundary>
      <QueryClientProvider client={queryClient}>
        <I18nProvider>{children}</I18nProvider>
      </QueryClientProvider>
    </ErrorBoundary>
  );
};
