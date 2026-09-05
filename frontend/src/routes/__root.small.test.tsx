import { createMemoryHistory, RouterProvider } from "@tanstack/react-router";
import { render, screen } from "@testing-library/react";
import userEvent from "@testing-library/user-event";
import { describe, expect, test } from "vitest";
import { AppProviders } from "@/app/providers/AppProviders";
import { createAppRouter } from "@/lib/router";

describe("root route wiring", () => {
  test("renders the home screen inside the shared application shell", async () => {
    const history = createMemoryHistory({ initialEntries: ["/"] });
    const router = createAppRouter({ history });

    render(
      <AppProviders>
        <RouterProvider router={router} />
      </AppProviders>,
    );

    expect(await screen.findByRole("banner")).toBeVisible();
    expect(
      await screen.findByRole("heading", { name: "文字で、文字を描く。" }),
    ).toBeVisible();
    expect(await screen.findByRole("contentinfo")).toBeVisible();
  });

  test("renders the 404 view inside the shared application shell", async () => {
    const history = createMemoryHistory({ initialEntries: ["/missing"] });
    const router = createAppRouter({ history });

    render(
      <AppProviders>
        <RouterProvider router={router} />
      </AppProviders>,
    );

    expect(await screen.findByRole("heading", { name: "404" })).toBeVisible();
    expect(await screen.findByRole("banner")).toBeVisible();
    expect(await screen.findByRole("contentinfo")).toBeVisible();
  });

  test("navigates from the 404 recovery link to the home screen", async () => {
    const user = userEvent.setup();
    const history = createMemoryHistory({ initialEntries: ["/missing"] });
    const router = createAppRouter({ history });

    render(
      <AppProviders>
        <RouterProvider router={router} />
      </AppProviders>,
    );

    const homeLink = await screen.findByRole("link", {
      name: "トップページへ戻る",
    });
    expect(homeLink).toHaveAttribute("href", "/");

    await user.click(homeLink);

    expect(
      await screen.findByRole("heading", { name: "文字で、文字を描く。" }),
    ).toBeVisible();
  });

  // ID: ROOT-ROUTE-S-004
  // Source: docs/v1/ui/ui.md § 20; docs/v1/ui/components/ErrorFallback.md; docs/v1/ui/components/App.md
  // Given: A child rendered by the application throws during rendering
  // When: The root ErrorBoundary handles the exception
  // Then: ErrorFallback replaces the application content without the shared header or footer
  // Blocked by: AppProviders ErrorBoundary and RouterProvider integration
  // Priority: P0
  test.todo("renders the error fallback outside the shared application shell");
});
