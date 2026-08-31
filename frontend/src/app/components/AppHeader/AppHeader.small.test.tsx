import { render, screen } from "@testing-library/react";
import userEvent from "@testing-library/user-event";
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

  test("updates its displayed copy after the user changes locale", async () => {
    const user = userEvent.setup();
    render(
      <I18nProvider>
        <AppHeader />
      </I18nProvider>,
    );

    await user.click(screen.getByRole("button", { name: "日本語" }));
    await user.click(screen.getByRole("menuitem", { name: "English" }));

    expect(screen.getByRole("button", { name: "English" })).toBeVisible();
  });
});
