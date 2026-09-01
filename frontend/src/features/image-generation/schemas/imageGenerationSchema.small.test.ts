import { describe, expect, test } from "vitest";
import { imageGenerationSchema } from "./imageGenerationSchema";

describe("imageGenerationSchema validation contract", () => {
  const validInput = {
    text: "KA",
    foregroundCharacter: "🌻",
    foregroundColor: "#FFD400",
    backgroundCharacter: "☀",
    backgroundColor: "#FF69B4",
    type: "standard",
  } as const;

  // ID: IMAGE-GENERATION-SCHEMA-S-001
  // Source: docs/v1/ui/components/ImageGenerationForm.md § Validation schema; docs/v1/ui/ui.md § 11
  // Given: The form contains the minimum valid text, rendering character, colors, and supported image type
  // When: The values are validated
  // Then: Validation succeeds without reporting a validation error
  // Priority: P0
  test("accepts a complete valid image generation request", () => {
    const result = imageGenerationSchema.safeParse(validInput);

    expect(result.success).toBe(true);
  });

  describe("text-to-render", () => {
    // ID: IMAGE-GENERATION-SCHEMA-S-002
    // Source: docs/v1/ui/ui.md § 11 "Text to Render"
    // Given: The text-to-render field is empty
    // When: The values are validated
    // Then: Validation reports that the text-to-render field is required
    // Error: required text-to-render value
    // Priority: P0
    test("rejects an empty text-to-render value", () => {
      expect(
        imageGenerationSchema.safeParse({ ...validInput, text: "" }).success,
      ).toBe(false);
    });

    // ID: IMAGE-GENERATION-SCHEMA-S-003
    // Source: docs/v1/ui/ui.md § 11 "Text to Render"
    // Given: The text-to-render value contains exactly 64 characters
    // When: The values are validated
    // Then: Validation succeeds
    // Priority: P1
    test("accepts a text-to-render value at the 64-character limit", () => {
      expect(
        imageGenerationSchema.safeParse({ ...validInput, text: "a".repeat(64) })
          .success,
      ).toBe(true);
    });

    // ID: IMAGE-GENERATION-SCHEMA-S-004
    // Source: docs/v1/ui/ui.md § 11 "Text to Render"
    // Given: The text-to-render value contains 65 characters
    // When: The values are validated
    // Then: Validation reports that the maximum length was exceeded
    // Error: text-to-render length limit
    // Priority: P0
    test("rejects a text-to-render value over 64 characters", () => {
      expect(
        imageGenerationSchema.safeParse({ ...validInput, text: "a".repeat(65) })
          .success,
      ).toBe(false);
    });

    // ID: IMAGE-GENERATION-SCHEMA-S-005
    // Source: docs/v1/ui/ui.md § 11 "Text to Render"
    // Given: The text-to-render value consists only of whitespace characters
    // When: The values are validated
    // Then: Validation reports that a displayable character is required
    // Error: whitespace-only text-to-render value
    // Priority: P0
    test("rejects a whitespace-only text-to-render value", () => {
      expect(
        imageGenerationSchema.safeParse({ ...validInput, text: " \t\n" })
          .success,
      ).toBe(false);
    });

    // ID: IMAGE-GENERATION-SCHEMA-S-006
    // Source: docs/v1/ui/ui.md § 11 "Text to Render"
    // Given: The text-to-render value contains a control character
    // When: The values are validated
    // Then: Validation reports that control characters are not allowed
    // Error: control character in text-to-render value
    // Priority: P0
    test("rejects control characters in the text-to-render value", () => {
      expect(
        imageGenerationSchema.safeParse({ ...validInput, text: "KA\u0000" })
          .success,
      ).toBe(false);
    });
  });

  describe("rendering-character", () => {
    // ID: IMAGE-GENERATION-SCHEMA-S-007
    // Source: docs/v1/ui/ui.md § 11 "Character Used to Render Text"
    // Given: The rendering-character field is empty
    // When: The values are validated
    // Then: Validation reports that the rendering character is required
    // Error: required rendering-character value
    // Priority: P0
    test("rejects an empty rendering-character value", () => {
      expect(
        imageGenerationSchema.safeParse({
          ...validInput,
          foregroundCharacter: "",
        }).success,
      ).toBe(false);
    });

    // ID: IMAGE-GENERATION-SCHEMA-S-008
    // Source: docs/v1/ui/ui.md § 11 "Character Used to Render Text"
    // Given: The rendering-character value contains exactly 128 characters
    // When: The values are validated
    // Then: Validation succeeds
    // Priority: P1
    test("accepts a rendering-character value at the 128-character limit", () => {
      expect(
        imageGenerationSchema.safeParse({
          ...validInput,
          foregroundCharacter: "a".repeat(128),
        }).success,
      ).toBe(true);
    });

    // ID: IMAGE-GENERATION-SCHEMA-S-009
    // Source: docs/v1/ui/ui.md § 11 "Character Used to Render Text"
    // Given: The rendering-character value contains 129 characters
    // When: The values are validated
    // Then: Validation reports that the maximum length was exceeded
    // Error: rendering-character length limit
    // Priority: P0
    test("rejects a rendering-character value over 128 characters", () => {
      expect(
        imageGenerationSchema.safeParse({
          ...validInput,
          foregroundCharacter: "a".repeat(129),
        }).success,
      ).toBe(false);
    });

    // ID: IMAGE-GENERATION-SCHEMA-S-010
    // Source: docs/v1/ui/ui.md § 11 "Character Used to Render Text"
    // Given: The rendering-character value contains a control character
    // When: The values are validated
    // Then: Validation reports that control characters are not allowed
    // Error: control character in rendering-character value
    // Priority: P0
    test("rejects control characters in the rendering-character value", () => {
      expect(
        imageGenerationSchema.safeParse({
          ...validInput,
          foregroundCharacter: "a\u0000",
        }).success,
      ).toBe(false);
    });
  });

  describe("background-character", () => {
    // ID: IMAGE-GENERATION-SCHEMA-S-011
    // Source: docs/v1/ui/ui.md § 11 "Background Character"
    // Given: The background-character field is empty
    // When: The values are validated
    // Then: Validation reports that the background character is required
    // Error: required background-character value
    // Priority: P0
    test("rejects an empty background-character value", () => {
      expect(
        imageGenerationSchema.safeParse({
          ...validInput,
          backgroundCharacter: "",
        }).success,
      ).toBe(false);
    });

    // ID: IMAGE-GENERATION-SCHEMA-S-012
    // Source: docs/v1/ui/ui.md § 11 "Background Character"
    // Given: The background-character value contains exactly 128 characters
    // When: The values are validated
    // Then: Validation succeeds
    // Priority: P1
    test("accepts a background-character value at the 128-character limit", () => {
      expect(
        imageGenerationSchema.safeParse({
          ...validInput,
          backgroundCharacter: "a".repeat(128),
        }).success,
      ).toBe(true);
    });

    // ID: IMAGE-GENERATION-SCHEMA-S-013
    // Source: docs/v1/ui/ui.md § 11 "Background Character"
    // Given: The background-character value contains 129 characters
    // When: The values are validated
    // Then: Validation reports that the maximum length was exceeded
    // Error: background-character length limit
    // Priority: P0
    test("rejects a background-character value over 128 characters", () => {
      expect(
        imageGenerationSchema.safeParse({
          ...validInput,
          backgroundCharacter: "a".repeat(129),
        }).success,
      ).toBe(false);
    });

    // ID: IMAGE-GENERATION-SCHEMA-S-014
    // Source: docs/v1/ui/ui.md § 11 "Background Character"
    // Given: The background-character value contains a control character
    // When: The values are validated
    // Then: Validation reports that control characters are not allowed
    // Error: control character in background-character value
    // Priority: P0
    test("rejects control characters in the background-character value", () => {
      expect(
        imageGenerationSchema.safeParse({
          ...validInput,
          backgroundCharacter: "a\u0000",
        }).success,
      ).toBe(false);
    });
  });

  describe("character combination", () => {
    // ID: IMAGE-GENERATION-SCHEMA-S-015
    // Source: docs/v1/ui/ui.md § 11 "Character Combination"
    // Given: Both character fields consist only of whitespace characters
    // When: The values are validated
    // Then: Validation reports that at least one field must contain a displayable character
    // Error: whitespace-only character combination
    // Priority: P0
    test("rejects a whitespace-only rendering and background character combination", () => {
      expect(
        imageGenerationSchema.safeParse({
          ...validInput,
          foregroundCharacter: " ",
          backgroundCharacter: "\t",
        }).success,
      ).toBe(false);
    });

    // ID: IMAGE-GENERATION-SCHEMA-S-016
    // Source: docs/v1/ui/ui.md § 11 "Character Combination"
    // Given: The rendering-character field is whitespace-only and the background-character field contains a displayable character
    // When: The values are validated
    // Then: Validation succeeds
    // Priority: P1
    test("accepts a displayable background character when the rendering character is whitespace-only", () => {
      expect(
        imageGenerationSchema.safeParse({
          ...validInput,
          foregroundCharacter: " ",
          backgroundCharacter: "☀",
        }).success,
      ).toBe(true);
    });

    // ID: IMAGE-GENERATION-SCHEMA-S-017
    // Source: docs/v1/ui/ui.md § 11 "Character Combination"
    // Given: The background-character field is whitespace-only and the rendering-character field contains a displayable character
    // When: The values are validated
    // Then: Validation succeeds
    // Priority: P1
    test("accepts a displayable rendering character when the background character is whitespace-only", () => {
      expect(
        imageGenerationSchema.safeParse({
          ...validInput,
          foregroundCharacter: "🌻",
          backgroundCharacter: " ",
        }).success,
      ).toBe(true);
    });
  });

  describe("image type", () => {
    // ID: IMAGE-GENERATION-SCHEMA-S-018
    // Source: docs/v1/ui/ui.md § 6; docs/v1/ui/components/ImageGenerationForm.md § Validation schema
    // Given: The image type is one of the supported API values
    // When: The values are validated
    // Then: Validation succeeds for each supported image type
    // Priority: P1
    test("accepts every supported image type value", () => {
      for (const type of ["standard", "x-background", "x-icon"] as const) {
        expect(
          imageGenerationSchema.safeParse({ ...validInput, type }).success,
        ).toBe(true);
      }
    });

    // ID: IMAGE-GENERATION-SCHEMA-S-019
    // Source: docs/v1/ui/ui.md § 6; docs/v1/ui/components/ImageGenerationForm.md § Validation schema
    // Given: The image type is not one of the supported API values
    // When: The values are validated
    // Then: Validation reports an invalid image type
    // Error: unsupported image type
    // Priority: P0
    test("rejects an unsupported image type value", () => {
      expect(
        imageGenerationSchema.safeParse({ ...validInput, type: "unknown" })
          .success,
      ).toBe(false);
    });
  });
});
