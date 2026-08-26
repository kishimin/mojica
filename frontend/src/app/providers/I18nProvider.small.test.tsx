import { render, screen } from "@testing-library/react";
import { beforeEach, describe, expect, test } from "vitest";

import { I18nProvider, useI18n } from "./I18nProvider";

const LocaleConsumer = () => {
  const { locale } = useI18n();

  return <output>{locale}</output>;
};

describe("I18nProvider locale contract", () => {
  beforeEach(() => {
    localStorage.clear();
  });

  // ID: FOUNDATION-I18N-S-001
  // Source: docs/v1/ui/component-design.md §1 and ui.md §13
  // Given: no locale value has been persisted
  // When: the internationalization provider initializes
  // Then: Japanese is exposed as the default UI locale
  // Blocked by: I18nProvider implementation
  // Priority: P0
  test("uses Japanese when no locale has been persisted", () => {
    render(
      <I18nProvider>
        <LocaleConsumer />
      </I18nProvider>,
    );

    expect(screen.getByText("ja")).toBeInTheDocument();
  });

  // ID: FOUNDATION-I18N-S-002
  // Source: docs/v1/ui/component-design.md §1 and frontend-architecture.md §Routing
  // Given: English is stored under the locale key
  // When: the internationalization provider initializes
  // Then: English is exposed to consumers
  // Blocked by: I18nProvider implementation
  // Priority: P0
  test("restores persisted English as the active locale", () => {
    localStorage.setItem("locale", "en");

    render(
      <I18nProvider>
        <LocaleConsumer />
      </I18nProvider>,
    );

    expect(screen.getByText("en")).toBeInTheDocument();
  });

  // ID: FOUNDATION-I18N-S-003
  // Source: docs/v1/ui/component-design.md §1 and frontend-architecture.md §Routing
  // Given: an unsupported value is stored under the locale key
  // When: the internationalization provider initializes
  // Then: Japanese is exposed instead of the unsupported value
  // Error: unsupported persisted locale
  // Priority: P1
  test.todo("falls back to Japanese for an unsupported persisted locale");

  // ID: FOUNDATION-I18N-S-004
  // Source: docs/v1/ui/ui.md §13 Internationalization
  // Given: the provider currently exposes Japanese
  // When: a consumer selects English
  // Then: English is exposed and persisted under the locale key
  // Blocked by: I18nProvider implementation
  // Priority: P0
  test.todo("updates and persists a supported locale selected by a consumer");
});
