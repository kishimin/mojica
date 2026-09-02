export type ImageGenerationErrorPresentation =
  | "requestError"
  | "requestLimit"
  | "fallback";

export const toImageGenerationErrorPresentation = (
  code: string | null | undefined,
): ImageGenerationErrorPresentation => {
  switch (code) {
    case "BAD_REQUEST":
      return "requestError";
    case "RATE_LIMIT_EXCEEDED":
      return "requestLimit";
    default:
      return "fallback";
  }
};
