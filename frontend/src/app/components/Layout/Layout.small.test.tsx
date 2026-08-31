import { render, screen } from "@testing-library/react";
import { describe, expect, test, vi } from "vitest";
import Layout from "./Layout";

vi.mock("@tanstack/react-router", () => ({
  Outlet: () => <main>Matched child content</main>,
}));

describe("Layout", () => {
  test("wraps matched route content with the header and footer", () => {
    render(<Layout />);

    expect(screen.getByRole("banner")).toBeVisible();
    expect(
      screen.getByRole("main", { name: "Matched child content" }),
    ).toBeVisible();
    expect(screen.getByRole("contentinfo")).toBeVisible();
  });
});
