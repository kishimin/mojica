import { render, screen } from "@testing-library/react";
import userEvent from "@testing-library/user-event";
import { describe, expect, test } from "vitest";
import TextField from "./TextField";

describe("TextField", () => {
  test("accepts text through the labeled textbox", async () => {
    const user = userEvent.setup();
    render(<TextField label="Text" />);

    const textbox = screen.getByRole("textbox", { name: "Text" });
    await user.type(textbox, "Hello");

    expect(textbox).toHaveValue("Hello");
  });

  // ID: TEXT-FIELD-S-002
  // Source: docs/v1/ui/components/TextField.md § Storybook; docs/v1/ui/component-design.md § 4
  // Given: A text field has a validation error message
  // When: The user encounters the invalid field
  // Then: The textbox exposes the error message as its accessible description
  // Blocked by: TextField implementation
  // Priority: P0
  test.todo("associates the validation message with the textbox");

  // ID: TEXT-FIELD-S-003
  // Source: docs/v1/ui/components/TextField.md § Props, § Storybook
  // Given: A text field is disabled and displays an existing value
  // When: The user attempts to type into the field
  // Then: The textbox remains disabled and its displayed value does not change
  // Blocked by: TextField implementation
  // Priority: P1
  test.todo("prevents editing when the textbox is disabled");

  // ID: TEXT-FIELD-S-004
  // Source: docs/v1/ui/components/TextField.md § Props
  // Given: The caller supplies a native placeholder attribute
  // When: The text field is rendered
  // Then: The textbox displays the supplied placeholder
  // Blocked by: TextField implementation
  // Priority: P1
  test.todo("displays the placeholder supplied by the caller");

  // ID: TEXT-FIELD-S-005
  // Source: docs/v1/ui/components/TextField.md § Props; docs/v1/ui/component-design.md § 4
  // Given: The caller supplies an accessible description and the field has a validation error
  // When: The text field is rendered
  // Then: The textbox exposes both the caller description and validation message as accessible descriptions
  // Blocked by: TextField implementation
  // Priority: P0
  test.todo("retains the caller description when adding the validation description");
});
