import { screen } from "@testing-library/react";
import { describe, expect, test } from "vitest";
import App from "./App";
import { setup } from "@/tests/test-utils";

describe("App", () => {
  test("renders the image-generation screen inside the application shell", () => {
    setup(<App />);

    expect(screen.getByRole("banner")).toBeVisible();
    expect(
      screen.getByRole("heading", { name: "文字で、文字を描く。" }),
    ).toBeVisible();
    expect(
      screen.getByRole("textbox", { name: "描画する文字列" }),
    ).toBeVisible();
    expect(
      screen.getByRole("button", { name: "画像を生成する" }),
    ).toBeEnabled();
    expect(screen.getByRole("contentinfo")).toBeVisible();
  });
});
