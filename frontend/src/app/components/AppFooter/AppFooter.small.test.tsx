import { createMemoryHistory, RouterProvider } from "@tanstack/react-router";
import { render, screen } from "@testing-library/react";
import { describe, expect, test } from "vitest";
import { createAppRouter } from "@/lib/router";
import { I18nProvider } from "@/providers/I18nProvider";

const renderFooter = (locale: "ja" | "en" = "ja") => {
  const router = createAppRouter({ history: createMemoryHistory({ initialEntries: ["/privacy"] }) });
  return render(<I18nProvider initialLocale={locale}><RouterProvider router={router} /></I18nProvider>);
};

describe("AppFooter", () => {
  test("renders the application footer as a contentinfo landmark", async () => {
    renderFooter();

    expect(await screen.findByRole("contentinfo")).toBeVisible();
  });

  test("displays the documented copyright text", async () => {
    renderFooter();

    expect(await screen.findByText("© kishimin 2026")).toBeVisible();
  });

  test("links to the contact form", async () => {
    renderFooter();

    expect(await screen.findByRole("link", { name: "お問い合わせ" })).toHaveAttribute(
      "href",
      "https://docs.google.com/forms/d/e/1FAIpQLSdG8CcjnmGBseY6ExWhEidXutkXzyU3VmVrhFfQs9crJeOndQ/viewform",
    );
  });

  test("shows the contact link in English for the English locale", async () => {
    renderFooter("en");

    expect(await screen.findByRole("link", { name: "Contact" })).toHaveAttribute(
      "href",
      "https://docs.google.com/forms/d/e/1FAIpQLSedj8RXI0wSVnIHYu4jyRi7oyZ9jRFH98QGLyXAHtHC10XqFw/viewform",
    );
  });
});
