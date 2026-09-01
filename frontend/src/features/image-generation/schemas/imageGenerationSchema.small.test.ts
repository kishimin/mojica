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

      expect(result.success).toBe(false);
      if (!result.success) {
        expect(result.error.issues.map(({ message }) => message)).toContain(
          "text.required",
        );
      }
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

      expect(result.success).toBe(false);
      if (!result.success) {
        expect(result.error.issues.map(({ message }) => message)).toContain(
          "text.maxLength",
        );
      }
    });

    test("rejects a whitespace-only text-to-render value", () => {
      const result = imageGenerationSchema.safeParse({
        ...validInput,
        text: " \t\n",
      });

      expect(result.success).toBe(false);
      if (!result.success) {
        expect(result.error.issues.map(({ message }) => message)).toContain(
          "text.required",
        );
      }
    });

    test("rejects control characters in the text-to-render value", () => {
      const result = imageGenerationSchema.safeParse({
        ...validInput,
        text: "KA\u0000",
      });

      expect(result.success).toBe(false);
      if (!result.success) {
        expect(result.error.issues.map(({ message }) => message)).toContain(
          "text.controlCharacter",
        );
      }
    });

    test("rejects trailing control characters in the text-to-render value", () => {
      const result = imageGenerationSchema.safeParse({
        ...validInput,
        text: "KA\t",
      });

      expect(result.success).toBe(false);
      if (!result.success) {
        expect(result.error.issues.map(({ message }) => message)).toContain(
          "text.controlCharacter",
        );
      }
    });
  });

  describe("rendering-character", () => {
    test("rejects an empty rendering-character value", () => {
      const result = imageGenerationSchema.safeParse({
        ...validInput,
        foregroundCharacter: "",
      });

      expect(result.success).toBe(false);
      if (!result.success) {
        expect(result.error.issues.map(({ message }) => message)).toContain(
          "foregroundCharacter.required",
        );
      }
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

      expect(result.success).toBe(false);
      if (!result.success) {
        expect(result.error.issues.map(({ message }) => message)).toContain(
          "foregroundCharacter.maxLength",
        );
      }
    });

    test("rejects control characters in the rendering-character value", () => {
      const result = imageGenerationSchema.safeParse({
        ...validInput,
        foregroundCharacter: "a\u0000",
      });

      expect(result.success).toBe(false);
      if (!result.success) {
        expect(result.error.issues.map(({ message }) => message)).toContain(
          "foregroundCharacter.controlCharacter",
        );
      }
    });
  });

  describe("background-character", () => {
    test("rejects an empty background-character value", () => {
      const result = imageGenerationSchema.safeParse({
        ...validInput,
        backgroundCharacter: "",
      });

      expect(result.success).toBe(false);
      if (!result.success) {
        expect(result.error.issues.map(({ message }) => message)).toContain(
          "backgroundCharacter.required",
        );
      }
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

      expect(result.success).toBe(false);
      if (!result.success) {
        expect(result.error.issues.map(({ message }) => message)).toContain(
          "backgroundCharacter.maxLength",
        );
      }
    });

    test("rejects control characters in the background-character value", () => {
      const result = imageGenerationSchema.safeParse({
        ...validInput,
        backgroundCharacter: "a\u0000",
      });

      expect(result.success).toBe(false);
      if (!result.success) {
        expect(result.error.issues.map(({ message }) => message)).toContain(
          "backgroundCharacter.controlCharacter",
        );
      }
    });
  });

  describe("character combination", () => {
    test("rejects a whitespace-only rendering and background character combination", () => {
      const result = imageGenerationSchema.safeParse({
        ...validInput,
        foregroundCharacter: " ",
        backgroundCharacter: "\t",
      });

      expect(result.success).toBe(false);
      if (!result.success) {
        expect(result.error.issues.map(({ message }) => message)).toContain(
          "characterCombination.required",
        );
      }
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
