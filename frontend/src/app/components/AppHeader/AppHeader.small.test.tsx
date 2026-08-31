import { screen } from "@testing-library/react";
import { describe, expect, test } from "vitest";
import AppHeader from "./AppHeader";
import { renderWithI18n } from "@/app/test-utils";
import { setupUser } from "@/tests/test-utils";

describe("AppHeader", () => {
  test("renders the application header as a banner landmark", () => {
    renderWithI18n(<AppHeader />);

    expect(screen.getByRole("banner")).toBeVisible();
  });

  test("displays the logo and copy for the current locale", () => {
    renderWithI18n(<AppHeader />);

    expect(screen.getByText("mojica")).toBeVisible();
    expect(screen.getByRole("button", { name: "日本語" })).toBeVisible();
  });

  test("updates its displayed copy after the user changes locale", async () => {
    const user = setupUser();
    renderWithI18n(<AppHeader />);

    await user.click(screen.getByRole("button", { name: "日本語" }));
    await user.click(screen.getByRole("menuitem", { name: "English" }));

    expect(screen.getByRole("button", { name: "English" })).toBeVisible();
  });
});
