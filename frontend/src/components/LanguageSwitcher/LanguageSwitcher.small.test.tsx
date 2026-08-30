import { describe, test } from "vitest";

describe("LanguageSwitcher", () => {
  // ID: LANGUAGE-SWITCHER-S-001
  // Source: docs/v1/ui/components/LanguageSwitcher.md § Storybook, § Tests
  // Given: The controlled locale is Japanese
  // When: The language switcher is rendered closed
  // Then: The selected Japanese language name is displayed
  // Blocked by: LanguageSwitcher implementation
  // Priority: P0
  test.todo("displays the controlled locale while closed");

  // ID: LANGUAGE-SWITCHER-S-002
  // Source: docs/v1/ui/components/LanguageSwitcher.md § Storybook, § Tests
  // Given: The language switcher is closed and has Japanese and English options
  // When: The user opens it from the keyboard
  // Then: A menu exposes both language options
  // Blocked by: LanguageSwitcher implementation
  // Priority: P0
  test.todo("opens an accessible language menu from the keyboard");

  // ID: LANGUAGE-SWITCHER-S-003
  // Source: docs/v1/ui/components/LanguageSwitcher.md § Storybook, § Tests
  // Given: The language menu is open with Japanese selected
  // When: The user moves to English and confirms the option with the keyboard
  // Then: The component reports English as the requested locale
  // Blocked by: LanguageSwitcher implementation
  // Priority: P0
  test.todo("reports the language selected with the keyboard");

  // ID: LANGUAGE-SWITCHER-S-004
  // Source: docs/v1/ui/components/LanguageSwitcher.md § Storybook, § Tests
  // Given: The language menu is open
  // When: The user presses Escape
  // Then: The menu closes and the selected language remains Japanese
  // Blocked by: LanguageSwitcher implementation
  // Priority: P1
  test.todo("keeps the selected language when the menu is dismissed");
});
