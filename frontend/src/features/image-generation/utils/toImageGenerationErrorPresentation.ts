import type { ApiErrorResponse } from "@/models/apiErrorResponse";
import {
  imageGenerationErrorPresentations,
  type ImageGenerationErrorPresentation,
} from "@/types/image-generation-error-presentation";

/** Provides the module's public behavior. */
export const toImageGenerationErrorPresentation = (
  code: ApiErrorResponse["code"],
): ImageGenerationErrorPresentation => {
  switch (code) {
    case "BAD_REQUEST":
      return imageGenerationErrorPresentations.requestError;
    case "RATE_LIMIT_EXCEEDED":
      return imageGenerationErrorPresentations.requestLimit;
    case "INTERNAL_SERVER_ERROR":
      return imageGenerationErrorPresentations.serverError;
    case "IMAGE_GENERATION_FAILED":
      return imageGenerationErrorPresentations.imageGenerationServiceError;
    case "IMAGE_GENERATION_TIMEOUT":
      return imageGenerationErrorPresentations.timeout;
    case null:
    case undefined:
    default:
      return imageGenerationErrorPresentations.fallback;
  }
};
