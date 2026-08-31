import { render, screen } from "@testing-library/react";
import { describe, expect, test, vi } from "vitest";
import Layout from "./Layout";

vi.mock("@tanstack/react-router", () => ({
  Outlet: () => <main>Matched child content</main>,
}));

vi.mock("../AppHeader/AppHeader", () => ({
  default: () => <header />,
}));

vi.mock("../AppFooter/AppFooter", () => ({
  default: () => <footer />,
}));

describe("Layout", () => {
  test("wraps matched route content with the header and footer", () => {
    render(<Layout />);

    expect(screen.getByRole("banner")).toBeVisible();
    expect(screen.getByRole("main")).toHaveTextContent("Matched child content");
    expect(screen.getByRole("contentinfo")).toBeVisible();
  });
});
