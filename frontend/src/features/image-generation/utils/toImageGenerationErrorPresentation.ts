export const imageGenerationErrorPresentations = {
  requestError: "requestError",
  requestLimit: "requestLimit",
  serverError: "serverError",
  imageGenerationServiceError: "imageGenerationServiceError",
  timeout: "timeout",
  fallback: "fallback",
} as const;

export type ImageGenerationErrorPresentation =
  (typeof imageGenerationErrorPresentations)[keyof typeof imageGenerationErrorPresentations];

export const toImageGenerationErrorPresentation = (
  code: string | null | undefined,
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
