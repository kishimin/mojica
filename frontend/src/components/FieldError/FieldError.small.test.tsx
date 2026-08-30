import { render, screen } from "@testing-library/react";
import { describe, expect, test } from "vitest";
import FieldError from "./FieldError";

describe("FieldError", () => {
  test("displays the provided error message", () => {
    render(<FieldError message={"Name is required"} />);

    expect(screen.getByText("Name is required")).toBeVisible();
  });

  test("renders no error content for an empty message", () => {
    const { container } = render(<FieldError message={""} />);

    expect(container).toBeEmptyDOMElement();
  });
});
