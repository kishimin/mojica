export type ImageGenerationErrorPresentation =
  | "requestError"
  | "requestLimit"
  | "serverError"
  | "imageGenerationServiceError"
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
    default:
      return "fallback";
  }
};
