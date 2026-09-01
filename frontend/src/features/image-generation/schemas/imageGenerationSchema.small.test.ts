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
      expect(
        imageGenerationSchema.safeParse({ ...validInput, text: "" }).success,
      ).toBe(false);
    });

    test("accepts a text-to-render value at the 64-character limit", () => {
      expect(
        imageGenerationSchema.safeParse({ ...validInput, text: "a".repeat(64) })
          .success,
      ).toBe(true);
    });

    test("rejects a text-to-render value over 64 characters", () => {
      expect(
        imageGenerationSchema.safeParse({ ...validInput, text: "a".repeat(65) })
          .success,
      ).toBe(false);
    });

    test("rejects a whitespace-only text-to-render value", () => {
      expect(
        imageGenerationSchema.safeParse({ ...validInput, text: " \t\n" })
          .success,
      ).toBe(false);
    });

    test("rejects control characters in the text-to-render value", () => {
      expect(
        imageGenerationSchema.safeParse({ ...validInput, text: "KA\u0000" })
          .success,
      ).toBe(false);
    });
  });

  describe("rendering-character", () => {
    test("rejects an empty rendering-character value", () => {
      expect(
        imageGenerationSchema.safeParse({
          ...validInput,
          foregroundCharacter: "",
        }).success,
      ).toBe(false);
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
      expect(
        imageGenerationSchema.safeParse({
          ...validInput,
          foregroundCharacter: "a".repeat(129),
        }).success,
      ).toBe(false);
    });

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
    test("rejects an empty background-character value", () => {
      expect(
        imageGenerationSchema.safeParse({
          ...validInput,
          backgroundCharacter: "",
        }).success,
      ).toBe(false);
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
      expect(
        imageGenerationSchema.safeParse({
          ...validInput,
          backgroundCharacter: "a".repeat(129),
        }).success,
      ).toBe(false);
    });

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
    test("rejects a whitespace-only rendering and background character combination", () => {
      expect(
        imageGenerationSchema.safeParse({
          ...validInput,
          foregroundCharacter: " ",
          backgroundCharacter: "\t",
        }).success,
      ).toBe(false);
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
