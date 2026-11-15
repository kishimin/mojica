import { render, screen } from "@testing-library/react";
import { describe, expect, test } from "vitest";
import AppFooter from "./AppFooter";
import { setupWithI18n } from "@/app/tests/test-utils";
import { I18nProvider } from "@/providers/I18nProvider";

describe("AppFooter", () => {
  test("renders the application footer as a contentinfo landmark", () => {
    setupWithI18n(<AppFooter />);

    expect(screen.getByRole("contentinfo")).toBeVisible();
  });

  test("displays the documented copyright text", () => {
    setupWithI18n(<AppFooter />);

    expect(screen.getByText("© kishimin 2026")).toBeVisible();
  });

  test("links to the contact form", () => {
    setupWithI18n(<AppFooter />);

    expect(screen.getByRole("link", { name: "お問い合わせ" })).toHaveAttribute(
      "href",
      "https://docs.google.com/forms/d/e/1FAIpQLSdG8CcjnmGBseY6ExWhEidXutkXzyU3VmVrhFfQs9crJeOndQ/viewform",
    );
  });

  test("shows the contact link in English for the English locale", () => {
    render(
      <I18nProvider initialLocale={"en"}>
        <AppFooter />
      </I18nProvider>,
    );

    expect(screen.getByRole("link", { name: "Contact" })).toHaveAttribute(
      "href",
      "https://docs.google.com/forms/d/e/1FAIpQLSedj8RXI0wSVnIHYu4jyRi7oyZ9jRFH98QGLyXAHtHC10XqFw/viewform",
    );
  });
});
