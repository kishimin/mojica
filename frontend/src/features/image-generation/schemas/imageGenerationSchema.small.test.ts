import { describe, expect, test } from "vitest";
import { imageGenerationSchema } from "./imageGenerationSchema";

const hasValidationMessage = (
  result: ReturnType<typeof imageGenerationSchema.safeParse>,
  message: string,
) => {
  return (
    !result.success &&
    result.error.issues.some(
      ({ message: issueMessage }) => issueMessage === message,
    )
  );
};

describe("imageGenerationSchema validation contract", () => {
  const validInput = {
    text: "KA",
    foregroundCharacter: "🌻",
    foregroundColor: "#FFD400",
    backgroundCharacter: "☀",
    backgroundColor: "#FF69B4",
    type: "standard",
  } as const;

  test("accepts a complete valid image generation request", () => {
    const result = imageGenerationSchema.safeParse(validInput);

    expect(result.success).toBe(true);
  });

  describe("text-to-render", () => {
    test("rejects an empty text-to-render value", () => {
      const result = imageGenerationSchema.safeParse({
        ...validInput,
        text: "",
      });

      expect(hasValidationMessage(result, "text.required")).toBe(true);
    });

    test("accepts a text-to-render value at the 64-character limit", () => {
      expect(
        imageGenerationSchema.safeParse({ ...validInput, text: "a".repeat(64) })
          .success,
      ).toBe(true);
    });

    test("rejects a text-to-render value over 64 characters", () => {
      const result = imageGenerationSchema.safeParse({
        ...validInput,
        text: "a".repeat(65),
      });

      expect(hasValidationMessage(result, "text.maxLength")).toBe(true);
    });

    test("rejects a whitespace-only text-to-render value", () => {
      const result = imageGenerationSchema.safeParse({
        ...validInput,
        text: " \t\n",
      });

      expect(hasValidationMessage(result, "text.required")).toBe(true);
    });

    test("rejects control characters in the text-to-render value", () => {
      const result = imageGenerationSchema.safeParse({
        ...validInput,
        text: "KA\u0000",
      });

      expect(hasValidationMessage(result, "text.controlCharacter")).toBe(true);
    });

    test("rejects trailing control characters in the text-to-render value", () => {
      const result = imageGenerationSchema.safeParse({
        ...validInput,
        text: "KA\t",
      });

      expect(hasValidationMessage(result, "text.controlCharacter")).toBe(true);
    });
  });

  describe("rendering-character", () => {
    test("rejects an empty rendering-character value", () => {
      const result = imageGenerationSchema.safeParse({
        ...validInput,
        foregroundCharacter: "",
      });

      expect(hasValidationMessage(result, "foregroundCharacter.required")).toBe(
        true,
      );
    });

    test("accepts a rendering-character value at the 128-character limit", () => {
      expect(
        imageGenerationSchema.safeParse({
          ...validInput,
          foregroundCharacter: "a".repeat(128),
        }).success,
      ).toBe(true);
    });

    test("rejects a rendering-character value over 128 characters", () => {
      const result = imageGenerationSchema.safeParse({
        ...validInput,
        foregroundCharacter: "a".repeat(129),
      });

      expect(
        hasValidationMessage(result, "foregroundCharacter.maxLength"),
      ).toBe(true);
    });

    test("rejects control characters in the rendering-character value", () => {
      const result = imageGenerationSchema.safeParse({
        ...validInput,
        foregroundCharacter: "a\u0000",
      });

      expect(
        hasValidationMessage(result, "foregroundCharacter.controlCharacter"),
      ).toBe(true);
    });
  });

  describe("background-character", () => {
    test("rejects an empty background-character value", () => {
      const result = imageGenerationSchema.safeParse({
        ...validInput,
        backgroundCharacter: "",
      });

      expect(hasValidationMessage(result, "backgroundCharacter.required")).toBe(
        true,
      );
    });

    test("accepts a background-character value at the 128-character limit", () => {
      expect(
        imageGenerationSchema.safeParse({
          ...validInput,
          backgroundCharacter: "a".repeat(128),
        }).success,
      ).toBe(true);
    });

    test("rejects a background-character value over 128 characters", () => {
      const result = imageGenerationSchema.safeParse({
        ...validInput,
        backgroundCharacter: "a".repeat(129),
      });

      expect(
        hasValidationMessage(result, "backgroundCharacter.maxLength"),
      ).toBe(true);
    });

    test("rejects control characters in the background-character value", () => {
      const result = imageGenerationSchema.safeParse({
        ...validInput,
        backgroundCharacter: "a\u0000",
      });

      expect(
        hasValidationMessage(result, "backgroundCharacter.controlCharacter"),
      ).toBe(true);
    });
  });

  describe("character combination", () => {
    test("rejects a whitespace-only rendering and background character combination", () => {
      const result = imageGenerationSchema.safeParse({
        ...validInput,
        foregroundCharacter: " ",
        backgroundCharacter: "\t",
      });

      expect(
        hasValidationMessage(result, "characterCombination.required"),
      ).toBe(true);
    });

    test("accepts a displayable background character when the rendering character is whitespace-only", () => {
      expect(
        imageGenerationSchema.safeParse({
          ...validInput,
          foregroundCharacter: " ",
          backgroundCharacter: "☀",
        }).success,
      ).toBe(true);
    });

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
    test("accepts every supported image type value", () => {
      for (const type of ["standard", "x-background", "x-icon"] as const) {
        expect(
          imageGenerationSchema.safeParse({ ...validInput, type }).success,
        ).toBe(true);
      }
    });

    test("rejects an unsupported image type value", () => {
      expect(
        imageGenerationSchema.safeParse({ ...validInput, type: "unknown" })
          .success,
      ).toBe(false);
    });
  });
});
