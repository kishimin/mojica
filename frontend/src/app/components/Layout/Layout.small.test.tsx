import {
  createMemoryHistory,
  createRootRoute,
  createRoute,
  createRouter,
  RouterProvider,
} from "@tanstack/react-router";
import { screen } from "@testing-library/react";
import { describe, expect, test } from "vitest";
import Layout from "./Layout";
import { renderWithI18n } from "@/app/test-utils";

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

    renderWithI18n(<RouterProvider router={router} />);

    expect(await screen.findByRole("banner")).toBeVisible();
    expect(await screen.findByRole("main")).toHaveTextContent(
      "Matched child content",
    );
    expect(await screen.findByRole("contentinfo")).toBeVisible();
  });
});
