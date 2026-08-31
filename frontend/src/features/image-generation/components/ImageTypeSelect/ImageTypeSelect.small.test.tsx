import { screen } from "@testing-library/react";
import { describe, expect, test, vi } from "vitest";

import { setup } from "@/tests/test-utils";

import ImageTypeSelect from "./ImageTypeSelect";

describe("ImageTypeSelect", () => {
  // ID: IMAGE-TYPE-SELECT-S-000
  // Source: docs/v1/ui/components/ImageTypeSelect.md § Storybook, § Tests
  // Given: The image-type selector is rendered with the supported API values
  // When: The user opens the selector
  // Then: The localized option labels are displayed in the documented order
  // Blocked by: ImageTypeSelect implementation
  // Priority: P0
  test("displays every supported image type option in order when opened", async () => {
    const { user } = setup(
      <ImageTypeSelect value={"standard"} onChange={vi.fn()} />,
    );

    await user.click(screen.getByRole("combobox"));

    const options = screen.getAllByRole("option");

    expect(options).toHaveLength(3);
    expect(options.map((option) => option.textContent)).toEqual([
      "標準画像",
      "X背景画像",
      "Xアイコン画像",
    ]);
  });

  // ID: IMAGE-TYPE-SELECT-S-001
  // Source: docs/v1/ui/components/ImageTypeSelect.md § Storybook, § Tests
  // Given: The controlled image type is standard
  // When: The selector is rendered
  // Then: The localized label for the standard image type is displayed as selected
  // Blocked by: ImageTypeSelect implementation
  // Priority: P0
  test.todo("displays the localized label for the controlled image type");

  // ID: IMAGE-TYPE-SELECT-S-002
  // Source: docs/v1/ui/components/ImageTypeSelect.md § Storybook, § Tests
  // Given: The image-type selector is focused with standard selected
  // When: The user selects another image type with the keyboard
  // Then: The newly selected localized label is displayed
  // Blocked by: ImageTypeSelect implementation
  // Priority: P0
  test.todo("changes the selected image type through keyboard interaction");

  // ID: IMAGE-TYPE-SELECT-S-003
  // Source: docs/v1/ui/components/ImageTypeSelect.md § Props; docs/v1/ui/component-design.md § 4
  // Given: The image-type selector has a validation error message
  // When: The user encounters the invalid selector
  // Then: The error copy is displayed and exposed as its accessible description
  // Blocked by: ImageTypeSelect implementation
  // Priority: P0
  test.todo("associates the validation message with the selector");
});
