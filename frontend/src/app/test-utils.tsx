import { render as testingLibraryRender } from "@testing-library/react";
import type { ReactElement } from "react";
import { I18nProvider } from "./providers/I18nProvider";

export const renderWithI18n = (element: ReactElement) =>
  testingLibraryRender(element, { wrapper: I18nProvider });
