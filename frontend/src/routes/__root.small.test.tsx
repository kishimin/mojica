import { createMemoryHistory, RouterProvider } from "@tanstack/react-router";
import { render, screen } from "@testing-library/react";
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

  // ID: ROOT-ROUTE-S-003
  // Source: docs/v1/ui/ui.md § 19; docs/v1/ui/components/NotFoundView.md
  // Given: The 404 view is displayed for an unknown path
  // When: The user inspects the recovery action
  // Then: An accessible home link points to the image-generation home path
  // Blocked by: root route notFoundComponent and NotFoundView link integration
  // Priority: P0
  test.todo("exposes an accessible link to the image-generation home path");

  // ID: ROOT-ROUTE-S-004
  // Source: docs/v1/ui/ui.md § 19; docs/v1/ui/components/NotFoundView.md
  // Given: The 404 view is displayed for an unknown path
  // When: The user activates the home recovery link
  // Then: The router navigates to the image-generation home path without an API request
  // Blocked by: root route, NotFoundView link, and router integration
  // Priority: P0
  test.todo("navigates from the 404 recovery link to the home screen");

  // ID: ROOT-ROUTE-S-005
  // Source: docs/v1/ui/ui.md § 20; docs/v1/ui/components/ErrorFallback.md; docs/v1/ui/components/App.md
  // Given: A child rendered by the application throws during rendering
  // When: The root ErrorBoundary handles the exception
  // Then: ErrorFallback replaces the application content without the shared header or footer
  // Blocked by: AppProviders ErrorBoundary and RouterProvider integration
  // Priority: P0
  test.todo("renders the error fallback outside the shared application shell");
});
