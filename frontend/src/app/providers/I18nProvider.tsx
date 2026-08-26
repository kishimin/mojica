import { createContext, type ReactNode, useContext } from "react";

type Locale = "ja" | "en";

type I18nContextValue = {
  locale: Locale;
};

type I18nProviderProps = {
  children: ReactNode;
};

const I18nContext = createContext<I18nContextValue | undefined>(undefined);

export const I18nProvider = ({ children }: I18nProviderProps) => (
  <I18nContext.Provider value={{ locale: "ja" }}>
    {children}
  </I18nContext.Provider>
);

export const useI18n = () => {
  const context = useContext(I18nContext);

  if (context === undefined) {
    throw new Error("useI18n must be used within I18nProvider");
  }

  return context;
};
