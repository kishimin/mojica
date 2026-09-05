import { screen } from "@testing-library/react";
import { describe, expect, test } from "vitest";
import App from "./App";
import { setup } from "@/tests/test-utils";

describe("App", () => {
  test("renders the image-generation screen inside the application shell", async () => {
    setup(<App />);

    expect(await screen.findByRole("banner")).toBeVisible();
    expect(
      await screen.findByRole("heading", { name: "文字で、文字を描く。" }),
    ).toBeVisible();
    expect(
      await screen.findByRole("textbox", { name: "描画する文字列" }),
    ).toBeVisible();
    expect(
      await screen.findByRole("button", { name: "画像を生成する" }),
    ).toBeEnabled();
    expect(await screen.findByRole("contentinfo")).toBeVisible();
  });
});
