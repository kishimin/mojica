import { render, screen } from "@testing-library/react";
import { describe, expect, test } from "vitest";
import ColorPickerField from "./ColorPickerField";

describe("ColorPickerField", () => {
  test("displays the initial HEX value in both color controls", () => {
    render(
      <ColorPickerField
        label={"Color"}
        value={"#FFD400"}
        onChange={() => undefined}
      />,
    );

    expect(screen.getByRole("textbox", { name: "Color" })).toHaveValue(
      "#FFD400",
    );
    expect(screen.getByLabelText("Color picker")).toHaveValue("#ffd400");
  });

  // ID: COLOR-PICKER-FIELD-S-002
  // Source: docs/v1/ui/components/ColorPickerField.md § Storybook, § Tests
  // Given: An enabled color-picker field is displayed
  // When: The user edits the HEX textbox
  // Then: Both controls display the edited HEX value
  // Blocked by: ColorPickerField implementation
  // Priority: P0
  test.todo("synchronizes the color control after editing the HEX textbox");

  // ID: COLOR-PICKER-FIELD-S-003
  // Source: docs/v1/ui/components/ColorPickerField.md § Storybook, § Tests
  // Given: An enabled color-picker field is displayed
  // When: The user chooses a value with the color control
  // Then: Both controls display the chosen HEX value
  // Blocked by: ColorPickerField implementation
  // Priority: P0
  test.todo("synchronizes the HEX textbox after choosing a color");

  // ID: COLOR-PICKER-FIELD-S-004
  // Source: docs/v1/ui/components/ColorPickerField.md § Storybook, § Tests
  // Given: A disabled color-picker field has an existing HEX value
  // When: The user attempts to edit either control
  // Then: Both controls remain disabled and their displayed value does not change
  // Blocked by: ColorPickerField implementation
  // Priority: P1
  test.todo("prevents value changes while disabled");

  // ID: COLOR-PICKER-FIELD-S-005
  // Source: docs/v1/ui/components/ColorPickerField.md § Props, § Tests; docs/v1/ui/component-design.md § 4
  // Given: A color-picker field has a validation error message
  // When: The user encounters the invalid field
  // Then: The error copy is displayed and exposed as an accessible description
  // Blocked by: ColorPickerField implementation
  // Priority: P0
  test.todo("associates the validation message with the field");
});
