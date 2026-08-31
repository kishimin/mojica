import { render, screen } from "@testing-library/react";
import { describe, expect, test } from "vitest";
import FieldError from "./FieldError";

describe("FieldError", () => {
  test("displays the provided error message", () => {
    render(<FieldError message={"Name is required"} />);

    const errorMessage = screen.getByText("Name is required");

    expect(errorMessage).toBeVisible();
    expect(errorMessage).toHaveClass("text-destructive");
  });

  test("renders no error content for an empty message", () => {
    const { container } = render(<FieldError message={""} />);

    expect(container).toBeEmptyDOMElement();
  });

  test("removes the displayed error when the message becomes empty", () => {
    const { rerender } = render(<FieldError message={"Name is required"} />);

    rerender(<FieldError message={""} />);

    expect(screen.queryByText("Name is required")).not.toBeInTheDocument();
  });
});
