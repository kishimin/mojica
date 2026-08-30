import { render, screen } from "@testing-library/react";
import { describe, expect, test } from "vitest";
import FieldError from "./FieldError";

describe("FieldError", () => {
  // ID: FIELD-ERROR-S-001
  // Source: docs/v1/ui/components/FieldError.md; FieldError display contract
  // Given: An error message is provided
  // When: FieldError is rendered
  // Then: The error message is visible to the user
  // Blocked by: FieldError implementation
  // Priority: P0
  test("displays the provided error message", () => {
    render(<FieldError message={"Name is required"} />);

    expect(screen.getByText("Name is required")).toBeVisible();
  });

  // ID: FIELD-ERROR-S-002
  // Source: docs/v1/ui/components/FieldError.md; FieldError empty-state contract
  // Given: An empty error message is provided
  // When: FieldError is rendered
  // Then: No error content is rendered
  // Blocked by: FieldError implementation
  // Priority: P0
  test("renders no error content for an empty message", () => {
    const { container } = render(<FieldError message={""} />);

    expect(container).toBeEmptyDOMElement();
  });
});
