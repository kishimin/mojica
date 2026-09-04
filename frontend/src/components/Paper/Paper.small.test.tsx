import { render, screen } from "@testing-library/react";
import { describe, expect, test } from "vitest";
import Paper from "./Paper";

describe("Paper", () => {
  test("renders its content inside the surface container", () => {
    render(
      <Paper>
        <p>{"Paper content"}</p>
      </Paper>,
    );

    expect(screen.getByText("Paper content")).toBeVisible();
  });
});
