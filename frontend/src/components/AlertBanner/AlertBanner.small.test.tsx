import { render, screen, within } from "@testing-library/react";
import { describe, expect, test } from "vitest";
import AlertBanner from "./AlertBanner";

describe("AlertBanner", () => {
  test("announces the provided title and description as an alert", () => {
    render(
      <AlertBanner
        title={"Image generation failed"}
        description={"Please try again later"}
      />,
    );

    const alert = screen.getByRole("alert");

    expect(within(alert).getByText("Image generation failed")).toBeVisible();
    expect(within(alert).getByText("Please try again later")).toBeVisible();
  });
});
