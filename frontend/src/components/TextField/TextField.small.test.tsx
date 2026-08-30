import { describe, test } from "vitest";

describe("TextField", () => {
  // ID: TEXT-FIELD-S-001
  // Source: docs/v1/ui/components/TextField.md § Storybook, § Tests
  // Given: A text field has a visible label and is enabled
  // When: The user types text into the field
  // Then: The field is discoverable by its textbox role and label, and displays the entered text
  // Blocked by: TextField implementation
  // Priority: P0
  test.todo("accepts text through the labeled textbox");

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
  // Given: The caller supplies name, placeholder, and aria-describedby attributes
  // When: The text field is rendered
  // Then: The textbox preserves the supplied name, placeholder, and accessible description
  // Blocked by: TextField implementation
  // Priority: P1
  test.todo("preserves native textbox attributes supplied by the caller");
});
