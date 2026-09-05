import { toImageGenerationErrorPresentation } from "./toImageGenerationErrorPresentation";
import {
  imageGenerationErrorMessages,
  type ImageGenerationErrorMessages,
} from "@/i18n/messages";
import type { ApiErrorResponse, ApiValidationErrorResponse } from "@/models";
import type { Locale } from "@/types/i18n";

type ImageGenerationApiError = {
  title: string;
  description: string;
};

/** Provides the module's public behavior. */
export const toImageGenerationApiError = (
  response: ApiErrorResponse | ApiValidationErrorResponse | undefined,
  locale: Locale,
): ImageGenerationApiError => {
  const presentation = toImageGenerationErrorPresentation(response?.code);
  const messages: ImageGenerationErrorMessages =
    imageGenerationErrorMessages[locale];

  return {
    title: messages[presentation],
    description: response?.message ?? "",
  };
};
