import { render, screen } from "@testing-library/react";
import userEvent from "@testing-library/user-event";
import { useState } from "react";
import { describe, expect, test, vi } from "vitest";
import LanguageSwitcher from "./LanguageSwitcher";
import type { Locale } from "@/types/i18n";

const languageOptions = [
  { locale: "ja" as const, label: "日本語" },
  { locale: "en" as const, label: "English" },
];

const ControlledLanguageSwitcher = () => {
  const [locale, setLocale] = useState<Locale>("ja");

  return (
    <LanguageSwitcher
      locale={locale}
      options={languageOptions}
      onChange={setLocale}
    />
  );
};

describe("LanguageSwitcher", () => {
  test("displays the controlled locale while closed", () => {
    render(
      <LanguageSwitcher
        locale={"ja"}
        options={languageOptions}
        onChange={vi.fn()}
      />,
    );

    expect(screen.getByRole("button", { name: "日本語" })).toBeVisible();
  });

  test("exposes all language options in order when opened from the keyboard", async () => {
    const user = userEvent.setup();
    const languageNames = languageOptions.map(({ label }) => label);
    render(
      <LanguageSwitcher
        locale={"ja"}
        options={languageOptions}
        onChange={vi.fn()}
      />,
    );

    await user.tab();
    await user.keyboard("{Enter}");

    const menuItems = screen.getAllByRole("menuitem");

    expect(menuItems).toHaveLength(languageNames.length);
    languageNames.forEach((languageName, index) => {
      expect(menuItems[index]).toHaveAccessibleName(languageName);
    });
  });

  test("updates the displayed language after keyboard selection", async () => {
    const user = userEvent.setup();
    render(<ControlledLanguageSwitcher />);

    await user.tab();
    await user.keyboard("{Enter}{ArrowDown}{Enter}");

    expect(screen.getByRole("button", { name: "English" })).toBeVisible();
  });

  // ID: LANGUAGE-SWITCHER-S-004
  // Source: docs/v1/ui/components/LanguageSwitcher.md § Storybook, § Tests
  // Given: The language menu is open
  // When: The user presses Escape
  // Then: The menu closes and the selected language remains Japanese
  // Blocked by: LanguageSwitcher implementation
  // Priority: P1
  test.todo("keeps the selected language when the menu is dismissed");
});
