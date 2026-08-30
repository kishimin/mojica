import { render, screen } from "@testing-library/react";
import { describe, expect, test } from "vitest";

import Logo from "./Logo";

describe("Logo", () => {
  test("renders the logo image with empty alternative text and the visible mojica wordmark", () => {
    render(<Logo />);

    expect(screen.getByRole("img", { name: "" })).toBeInTheDocument();
    expect(screen.getByText("mojica")).toBeInTheDocument();
  });
});
