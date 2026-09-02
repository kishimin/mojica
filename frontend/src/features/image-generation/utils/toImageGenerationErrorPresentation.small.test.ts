import { describe, expect, test } from "vitest";
import { toImageGenerationErrorPresentation } from "./toImageGenerationErrorPresentation";

describe("toImageGenerationErrorPresentation", () => {
  describe("supported API error codes", () => {
    // ID: IMAGE-GENERATION-ERROR-PRESENTATION-S-001
    // Source: docs/v1/ui/branch-plans/feat-mojica-ui-image-generation.md; docs/v1/ui/branch-plans/feat-mojica-ui-image-generation-work.md § 4; docs/v1/ui/ui.md § 12
    // Given: The API error code is BAD_REQUEST
    // When: The code is converted for form-level presentation
    // Then: The result identifies the localized "Request error" heading
    // Blocked by: toImageGenerationErrorPresentation implementation and error-heading i18n definitions
    // Priority: P1
    test("maps BAD_REQUEST to the request-error heading", () => {
      expect(toImageGenerationErrorPresentation("BAD_REQUEST")).toBe(
        "requestError",
      );
    });

    // ID: IMAGE-GENERATION-ERROR-PRESENTATION-S-002
    // Source: docs/v1/ui/branch-plans/feat-mojica-ui-image-generation.md; docs/v1/ui/branch-plans/feat-mojica-ui-image-generation-work.md § 4; docs/v1/ui/ui.md § 12
    // Given: The API error code is RATE_LIMIT_EXCEEDED
    // When: The code is converted for form-level presentation
    // Then: The result identifies the localized "Request limit exceeded" heading
    // Blocked by: toImageGenerationErrorPresentation implementation and error-heading i18n definitions
    // Priority: P1
    test("maps RATE_LIMIT_EXCEEDED to the request-limit heading", () => {
      expect(toImageGenerationErrorPresentation("RATE_LIMIT_EXCEEDED")).toBe(
        "requestLimit",
      );
    });

    // ID: IMAGE-GENERATION-ERROR-PRESENTATION-S-003
    // Source: docs/v1/ui/branch-plans/feat-mojica-ui-image-generation.md; docs/v1/ui/branch-plans/feat-mojica-ui-image-generation-work.md § 4; docs/v1/ui/ui.md § 12
    // Given: The API error code is INTERNAL_SERVER_ERROR
    // When: The code is converted for form-level presentation
    // Then: The result identifies the localized "Server error" heading
    // Blocked by: toImageGenerationErrorPresentation implementation and error-heading i18n definitions
    // Priority: P1
    test("maps INTERNAL_SERVER_ERROR to the server-error heading", () => {
      expect(toImageGenerationErrorPresentation("INTERNAL_SERVER_ERROR")).toBe(
        "serverError",
      );
    });

    // ID: IMAGE-GENERATION-ERROR-PRESENTATION-S-004
    // Source: docs/v1/ui/branch-plans/feat-mojica-ui-image-generation.md; docs/v1/ui/branch-plans/feat-mojica-ui-image-generation-work.md § 4; docs/v1/ui/ui.md § 12
    // Given: The API error code is IMAGE_GENERATION_FAILED
    // When: The code is converted for form-level presentation
    // Then: The result identifies the localized "Image generation service error" heading
    // Blocked by: toImageGenerationErrorPresentation implementation and error-heading i18n definitions
    // Priority: P1
    test("maps IMAGE_GENERATION_FAILED to the image-generation-service heading", () => {
      expect(
        toImageGenerationErrorPresentation("IMAGE_GENERATION_FAILED"),
      ).toBe("imageGenerationServiceError");
    });

    // ID: IMAGE-GENERATION-ERROR-PRESENTATION-S-005
    // Source: docs/v1/ui/branch-plans/feat-mojica-ui-image-generation.md; docs/v1/ui/branch-plans/feat-mojica-ui-image-generation-work.md § 4; docs/v1/ui/ui.md § 12
    // Given: The API error code is IMAGE_GENERATION_TIMEOUT
    // When: The code is converted for form-level presentation
    // Then: The result identifies the localized "Timeout" heading
    // Blocked by: toImageGenerationErrorPresentation implementation and error-heading i18n definitions
    // Priority: P1
    test("maps IMAGE_GENERATION_TIMEOUT to the timeout heading", () => {
      expect(
        toImageGenerationErrorPresentation("IMAGE_GENERATION_TIMEOUT"),
      ).toBe("timeout");
    });
  });

  describe("unsupported API error codes", () => {
    // ID: IMAGE-GENERATION-ERROR-PRESENTATION-S-006
    // Source: docs/v1/ui/branch-plans/feat-mojica-ui-image-generation.md; docs/v1/ui/branch-plans/feat-mojica-ui-image-generation-work.md § 4; docs/v1/ui/components/ImageGenerationForm.md § State model
    // Given: The API error code is absent or unsupported
    // When: The code is converted for form-level presentation
    // Then: The result identifies the localized fallback heading without exposing the internal code
    // Blocked by: toImageGenerationErrorPresentation implementation and error-heading i18n definitions
    // Priority: P1
    test("maps absent and unsupported API error codes to the safe fallback heading", () => {
      expect(toImageGenerationErrorPresentation("UNKNOWN_CODE")).toBe(
        "fallback",
      );
      expect(toImageGenerationErrorPresentation(undefined)).toBe("fallback");
    });
  });
});
