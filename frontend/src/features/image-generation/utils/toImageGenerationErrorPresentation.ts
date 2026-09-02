export type ImageGenerationErrorPresentation =
  | "requestError"
  | "requestLimit"
  | "serverError"
  | "imageGenerationServiceError"
  | "timeout"
  | "fallback";

export const toImageGenerationErrorPresentation = (
  code: string | null | undefined,
): ImageGenerationErrorPresentation => {
  switch (code) {
    case "BAD_REQUEST":
      return "requestError";
    case "RATE_LIMIT_EXCEEDED":
      return "requestLimit";
    case "INTERNAL_SERVER_ERROR":
      return "serverError";
    case "IMAGE_GENERATION_FAILED":
      return "imageGenerationServiceError";
    case "IMAGE_GENERATION_TIMEOUT":
      return "timeout";
    default:
      return "fallback";
  }
};
