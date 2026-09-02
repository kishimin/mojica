/** Image-generation error categories exposed to the UI layer. */
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
