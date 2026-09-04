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
});
