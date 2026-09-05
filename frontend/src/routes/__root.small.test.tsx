import { createMemoryHistory, RouterProvider } from "@tanstack/react-router";
import { screen } from "@testing-library/react";
import { describe, expect, test } from "vitest";
import { AppProviders } from "@/app/providers/AppProviders";
import { createAppRouter } from "@/lib/router";
import { setup } from "@/tests/test-utils";

const setupWithRouter = (initialEntry: string) => {
  const history = createMemoryHistory({ initialEntries: [initialEntry] });
  const router = createAppRouter({ history });

  return setup(
    <AppProviders>
      <RouterProvider router={router} />
    </AppProviders>,
  );
};

describe("root route wiring", () => {
  test("renders the home screen inside the shared application shell", async () => {
    setupWithRouter("/");

    expect(await screen.findByRole("banner")).toBeVisible();
    expect(
      await screen.findByRole("heading", { name: "文字で、文字を描く。" }),
    ).toBeVisible();
    expect(await screen.findByRole("contentinfo")).toBeVisible();
  });

  test("renders the 404 view inside the shared application shell", async () => {
    setupWithRouter("/missing");

    expect(await screen.findByRole("heading", { name: "404" })).toBeVisible();
    expect(await screen.findByRole("banner")).toBeVisible();
    expect(await screen.findByRole("contentinfo")).toBeVisible();
  });

  test("navigates from the 404 recovery link to the home screen", async () => {
    const { user } = setupWithRouter("/missing");

    const homeLink = await screen.findByRole("link", {
      name: "トップページへ戻る",
    });
    expect(homeLink).toHaveAttribute("href", "/");

    await user.click(homeLink);

    expect(
      await screen.findByRole("heading", { name: "文字で、文字を描く。" }),
    ).toBeVisible();
  });

  test("renders the error fallback outside the shared application shell", async () => {
    const ThrowingChild = () => {
      throw new Error("render failure");
    };
    localStorage.setItem("locale", "ja");

    setup(
      <AppProviders>
        <ThrowingChild />
      </AppProviders>,
    );

    expect(
      await screen.findByRole("heading", { name: "エラーが発生しました" }),
    ).toBeVisible();
    expect(screen.queryByRole("banner")).not.toBeInTheDocument();
    expect(screen.queryByRole("contentinfo")).not.toBeInTheDocument();
  });
});
