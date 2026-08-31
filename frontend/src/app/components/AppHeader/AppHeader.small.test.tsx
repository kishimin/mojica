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

  // ID: APP-HEADER-S-001
  // Source: docs/v1/ui/components/AppHeader.md § Storybook, § Tests
  // Given: The application locale is Japanese
  // When: The header is rendered
  // Then: The logo and Japanese language label are displayed
  // Blocked by: AppHeader implementation
  // Priority: P0
  test.todo("displays the logo and copy for the current locale");

  // ID: APP-HEADER-S-002
  // Source: docs/v1/ui/components/AppHeader.md § Responsibility, § Tests
  // Given: The header is displayed in Japanese
  // When: The user selects English through the language switcher
  // Then: The header displays the English language label from the i18n boundary
  // Blocked by: AppHeader and LanguageSwitcher implementation
  // Priority: P0
  test.todo("updates its displayed copy after the user changes locale");
});
