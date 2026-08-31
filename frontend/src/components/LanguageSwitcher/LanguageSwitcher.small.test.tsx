import { render, screen } from "@testing-library/react";
import { useState } from "react";
import { describe, expect, test, vi } from "vitest";
import { setupUser } from "@/tests/test-utils";
import LanguageSwitcher from "./LanguageSwitcher";
import type { Locale } from "@/types/i18n";

const expectedLanguageNames = ["日本語", "English"];

const ControlledLanguageSwitcher = () => {
  const [locale, setLocale] = useState<Locale>("ja");

  return <LanguageSwitcher locale={locale} onChange={setLocale} />;
};

describe("LanguageSwitcher", () => {
  test("displays the controlled locale while closed", () => {
    render(
      <LanguageSwitcher
        locale={"ja"}
        onChange={vi.fn<(locale: Locale) => void>()}
      />,
    );

    expect(screen.getByRole("button", { name: "日本語" })).toBeVisible();
  });

  test("exposes all language options in order when opened from the keyboard", async () => {
    const user = setupUser();
    render(
      <LanguageSwitcher
        locale={"ja"}
        onChange={vi.fn<(locale: Locale) => void>()}
      />,
    );

    await user.tab();
    await user.keyboard("{Enter}");

    const menuItems = screen.getAllByRole("menuitem");

    expect(menuItems).toHaveLength(expectedLanguageNames.length);
    expectedLanguageNames.forEach((languageName, index) => {
      expect(menuItems[index]).toHaveAccessibleName(languageName);
    });
  });

  test("updates the displayed language after keyboard selection", async () => {
    const user = setupUser();
    render(<ControlledLanguageSwitcher />);

    await user.tab();
    await user.keyboard("{Enter}{ArrowDown}{Enter}");

    expect(screen.getByRole("button", { name: "English" })).toBeVisible();
  });

  test("keeps the selected language when the menu is dismissed", async () => {
    const user = setupUser();
    render(<ControlledLanguageSwitcher />);

    await user.tab();
    await user.keyboard("{Enter}{Escape}");

    expect(screen.queryByRole("menu")).not.toBeInTheDocument();
    expect(screen.getByRole("button", { name: "日本語" })).toBeVisible();
  });
});
