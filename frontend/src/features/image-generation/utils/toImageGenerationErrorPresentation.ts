export type ImageGenerationErrorPresentation =
  | "requestError"
  | "requestLimit"
  | "serverError"
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
    default:
      return "fallback";
  }
};
