import { createContext } from "react";
import type { I18nContextType } from "../types/i18n";

export const I18nContext = createContext<I18nContextType | null>(null);
