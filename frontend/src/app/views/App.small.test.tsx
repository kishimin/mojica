import { screen } from "@testing-library/react";
import { describe, expect, test } from "vitest";
import App from "./App";
import { setup } from "@/tests/test-utils";

describe("App", () => {
  test("increments the counter each time the button is clicked", async () => {
    const { user } = setup(<App />);

    const button = screen.getByRole("button", { name: "Count is 0" });
    await user.click(button);

    expect(
      screen.getByRole("button", { name: "Count is 1" }),
    ).toBeInTheDocument();
  });
});
