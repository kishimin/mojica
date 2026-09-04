import { describe, expect, test } from "vitest";
import { toImageGenerationErrorPresentation } from "./toImageGenerationErrorPresentation";

describe("toImageGenerationErrorPresentation", () => {
  describe("supported API error codes", () => {
    test("maps BAD_REQUEST to the request-error heading", () => {
      expect(toImageGenerationErrorPresentation("BAD_REQUEST")).toBe(
        "requestError",
      );
    });

    test("maps RATE_LIMIT_EXCEEDED to the request-limit heading", () => {
      expect(toImageGenerationErrorPresentation("RATE_LIMIT_EXCEEDED")).toBe(
        "requestLimit",
      );
    });

    test("maps INTERNAL_SERVER_ERROR to the server-error heading", () => {
      expect(toImageGenerationErrorPresentation("INTERNAL_SERVER_ERROR")).toBe(
        "serverError",
      );
    });

    test("maps IMAGE_GENERATION_FAILED to the image-generation-service heading", () => {
      expect(
        toImageGenerationErrorPresentation("IMAGE_GENERATION_FAILED"),
      ).toBe("imageGenerationServiceError");
    });

    test("maps IMAGE_GENERATION_TIMEOUT to the timeout heading", () => {
      expect(
        toImageGenerationErrorPresentation("IMAGE_GENERATION_TIMEOUT"),
      ).toBe("timeout");
    });
  });

  describe("unsupported API error codes", () => {
    test("maps absent and unsupported API error codes to the safe fallback heading", () => {
      expect(toImageGenerationErrorPresentation("UNKNOWN_CODE")).toBe(
        "fallback",
      );
      expect(toImageGenerationErrorPresentation(undefined)).toBe("fallback");
    });
  });
});
