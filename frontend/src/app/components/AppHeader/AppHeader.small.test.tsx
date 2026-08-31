import { render, screen } from "@testing-library/react";
import { describe, expect, test } from "vitest";
import { I18nProvider } from "../../providers/I18nProvider";
import AppHeader from "./AppHeader";

describe("AppHeader", () => {
  test("renders the application header as a banner landmark", () => {
    render(
      <I18nProvider>
        <AppHeader />
      </I18nProvider>,
    );

    expect(screen.getByRole("banner")).toBeVisible();
  });

  test("displays the logo and copy for the current locale", () => {
    render(
      <I18nProvider>
        <AppHeader />
      </I18nProvider>,
    );

    expect(screen.getByText("mojica")).toBeVisible();
    expect(screen.getByRole("button", { name: "日本語" })).toBeVisible();
  });

  // ID: APP-HEADER-S-002
  // Source: docs/v1/ui/components/AppHeader.md § Responsibility, § Tests
  // Given: The header is displayed in Japanese
  // When: The user selects English through the language switcher
  // Then: The header displays the English language label from the i18n boundary
  // Blocked by: AppHeader and LanguageSwitcher implementation
  // Priority: P0
  test.todo("updates its displayed copy after the user changes locale");
});
