import { render, screen } from "@testing-library/react";
import {
  createMemoryHistory,
  createRootRoute,
  createRoute,
  createRouter,
  RouterProvider,
} from "@tanstack/react-router";
import { describe, expect, test } from "vitest";
import { I18nProvider } from "../../providers/I18nProvider";
import Layout from "./Layout";

const rootRoute = createRootRoute({ component: Layout });
const childRoute = createRoute({
  getParentRoute: () => rootRoute,
  path: "/",
  component: () => <main>{"Matched child content"}</main>,
});
const routeTree = rootRoute.addChildren([childRoute]);

describe("Layout", () => {
  test("wraps matched route content with the header and footer", async () => {
    const router = createRouter({
      routeTree,
      history: createMemoryHistory({ initialEntries: ["/"] }),
    });
    await router.load();

    render(
      <I18nProvider>
        <RouterProvider router={router} />
      </I18nProvider>,
    );

    expect(await screen.findByRole("banner")).toBeVisible();
    expect(await screen.findByRole("main")).toHaveTextContent(
      "Matched child content",
    );
    expect(await screen.findByRole("contentinfo")).toBeVisible();
  });
});
