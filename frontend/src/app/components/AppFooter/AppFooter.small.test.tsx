import { render, screen } from "@testing-library/react";
import { describe, expect, test } from "vitest";
import AppFooter from "./AppFooter";

describe("AppFooter", () => {
  test("renders the application footer as a contentinfo landmark", () => {
    render(<AppFooter />);

    expect(screen.getByRole("contentinfo")).toBeVisible();
  });

  test("displays the documented copyright text", () => {
    render(<AppFooter />);

    expect(screen.getByText("© kishimin 2026")).toBeVisible();
  });

  test("links to the contact form", () => {
    render(<AppFooter />);

    expect(screen.getByRole("link", { name: "お問い合わせ" })).toHaveAttribute(
      "href",
      "https://docs.google.com/forms/d/e/1FAIpQLSdG8CcjnmGBseY6ExWhEidXutkXzyU3VmVrhFfQs9crJeOndQ/viewform",
    );
  });
});
