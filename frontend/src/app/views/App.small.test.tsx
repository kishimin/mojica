import { render, screen } from "@testing-library/react";
import { describe, expect, test } from "vitest";
import { setupUser } from "@/tests/test-utils";
import App from "./App";

describe("App", () => {
  test("increments the counter each time the button is clicked", async () => {
    const user = setupUser();
    render(<App />);

    const button = screen.getByRole("button", { name: "Count is 0" });
    await user.click(button);

    expect(
      screen.getByRole("button", { name: "Count is 1" }),
    ).toBeInTheDocument();
  });
});
