import type { ReactElement } from "react";
import { I18nProvider } from "../../providers/I18nProvider";
import { setup } from "@/tests/test-utils";

export const setupWithI18n = (element: ReactElement) =>
  setup(element, { wrapper: I18nProvider });
