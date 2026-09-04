import { screen } from "@testing-library/react";
import { describe, expect, test } from "vitest";
import AppHeader from "./AppHeader";
import { setupWithI18n } from "@/app/tests/test-utils";

describe("AppHeader", () => {
  test("renders the application header as a banner landmark", () => {
    setupWithI18n(<AppHeader />);

    expect(screen.getByRole("banner")).toBeVisible();
  });

  test("displays the logo and copy for the current locale", () => {
    setupWithI18n(<AppHeader />);

    expect(screen.getByText("mojica")).toBeVisible();
    expect(screen.getByRole("button", { name: "日本語" })).toBeVisible();
  });

  test("updates its displayed copy after the user changes locale", async () => {
    const { user } = setupWithI18n(<AppHeader />);

    await user.click(screen.getByRole("button", { name: "日本語" }));
    await user.click(screen.getByRole("menuitem", { name: "English" }));

    expect(screen.getByRole("button", { name: "English" })).toBeVisible();
  });
});
