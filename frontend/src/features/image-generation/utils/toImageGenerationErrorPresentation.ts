export type ImageGenerationErrorPresentation =
  | "requestError"
  | "fallback";

export const toImageGenerationErrorPresentation = (
  code: string | null | undefined,
): ImageGenerationErrorPresentation => {
  switch (code) {
    case "BAD_REQUEST":
      return "requestError";
    default:
      return "fallback";
  }
};
