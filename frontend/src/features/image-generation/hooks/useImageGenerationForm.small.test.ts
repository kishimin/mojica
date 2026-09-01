import { renderHook } from "@testing-library/react";
import { describe, expect, test } from "vitest";
import { useImageGenerationForm } from "./useImageGenerationForm";

describe("useImageGenerationForm", () => {
  // ID: IMAGE-GENERATION-FORM-S-001
  // Source: docs/v1/ui/components/ImageGenerationForm.md § Tests; docs/v1/ui/ui.md § 6
  // Given: The image-generation form is initialized without caller-provided values
  // When: The form state is observed
  // Then: The form exposes the documented initial values, including the standard image type
  // Blocked by: useImageGenerationForm implementation
  // Priority: P0
  test("exposes the documented initial form values", () => {
    const { result } = renderHook(() => useImageGenerationForm());

    expect(result.current.getValues()).toEqual({
      text: "KA",
      foregroundCharacter: "🌻",
      foregroundColor: "#FFD400",
      backgroundCharacter: "☀",
      backgroundColor: "#FF69B4",
      type: "standard",
    });
  });

  // ID: IMAGE-GENERATION-FORM-S-002
  // Source: docs/v1/ui/components/ImageGenerationForm.md § Validation schema; docs/v1/ui/ui.md § 11
  // Given: The form contains a text value that violates the client validation contract
  // When: The form validates the current values
  // Then: The form exposes the corresponding validation message key for the text field
  // Error: text.required
  // Blocked by: useImageGenerationForm implementation
  // Priority: P0
  test.todo("exposes the schema validation message for an invalid text value");

  // ID: IMAGE-GENERATION-FORM-S-003
  // Source: docs/v1/ui/components/ImageGenerationForm.md § Validation schema; docs/v1/ui/ui.md § 11
  // Given: The form contains values that satisfy the image-generation validation contract
  // When: The form validates the current values
  // Then: The form reports no field validation errors
  // Blocked by: useImageGenerationForm implementation
  // Priority: P0
  test.todo("accepts valid image-generation form values");
});
