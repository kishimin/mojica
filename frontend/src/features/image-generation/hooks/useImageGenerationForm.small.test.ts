import { act, renderHook, waitFor } from "@testing-library/react";
import { describe, expect, test } from "vitest";
import { useImageGenerationForm } from "./useImageGenerationForm";
import { imageTypeDefinitions } from "@/types/image-type";

describe("useImageGenerationForm", () => {
  test("exposes the documented initial form values", () => {
    const { result } = renderHook(() => useImageGenerationForm());

    expect(result.current.getValues()).toEqual({
      text: "",
      foregroundCharacter: "",
      foregroundColor: "#000000",
      backgroundCharacter: "",
      backgroundColor: "#FFFFFF",
      type: imageTypeDefinitions.standard,
    });
  });

  test("exposes the schema validation message for an invalid text value", async () => {
    const { result } = renderHook(() => useImageGenerationForm());

    result.current.setValue("text", " ", { shouldValidate: true });

    await waitFor(() => {
      expect(result.current.formState.errors.text?.message).toBe(
        "text.required",
      );
    });
  });

  test("accepts valid image-generation form values", async () => {
    const { result } = renderHook(() => useImageGenerationForm());

    act(() => {
      result.current.reset({
        text: "KA",
        foregroundCharacter: "🌻",
        foregroundColor: "#FFD400",
        backgroundCharacter: "☀",
        backgroundColor: "#FF69B4",
        type: imageTypeDefinitions.standard,
      });
    });

    const isValid = await result.current.trigger();

    expect(isValid).toBe(true);
    expect(result.current.formState.errors).toEqual({});
  });
});
