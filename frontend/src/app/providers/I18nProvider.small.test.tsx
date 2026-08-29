import { render, screen } from "@testing-library/react";
import userEvent from "@testing-library/user-event";
import { beforeEach, describe, expect, test } from "vitest";
import { useI18n } from "../../hooks/use-i18n";
import { I18nProvider } from "./I18nProvider";

const LocaleConsumer = () => {
  const { locale } = useI18n();

  return <output>{locale}</output>;
};

const LocaleControlConsumer = () => {
  const { locale, setLocale } = useI18n();

  return <button onClick={() => setLocale("en")}>{locale}</button>;
};

describe("I18nProvider locale contract", () => {
  beforeEach(() => {
    localStorage.clear();
  });

  test("rejects consumers outside I18nProvider", () => {
    expect(() => render(<LocaleConsumer />)).toThrow(
      "useI18n must be used within I18nProvider",
    );
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
  test("falls back to Japanese for an unsupported persisted locale", () => {
    localStorage.setItem("locale", "fr");

    render(
      <I18nProvider>
        <LocaleConsumer />
      </I18nProvider>,
    );

    expect(screen.getByText("ja")).toBeInTheDocument();
  });

  // ID: FOUNDATION-I18N-S-004
  // Source: docs/v1/ui/ui.md §13 Internationalization
  // Given: the provider currently exposes Japanese
  // When: a consumer selects English
  // Then: English is exposed and persisted under the locale key
  // Blocked by: I18nProvider implementation
  // Priority: P0
  test("updates and persists a supported locale selected by a consumer", async () => {
    const user = userEvent.setup();
    render(
      <I18nProvider>
        <LocaleControlConsumer />
      </I18nProvider>,
    );

    await user.click(screen.getByRole("button", { name: "ja" }));

    expect(screen.getByRole("button", { name: "en" })).toBeInTheDocument();
    expect(localStorage.getItem("locale")).toBe("en");
  });
});
