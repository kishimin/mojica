import type { UseFormSetError } from "react-hook-form";
import type { ImageGenerationFormValues } from "../schemas/imageGenerationSchema";
import type { ApiValidationFieldError } from "@/models/apiValidationFieldError";

export const applyImageGenerationFieldErrors = (
  fieldErrors: ApiValidationFieldError[] | null | undefined,
  setError: UseFormSetError<ImageGenerationFormValues>,
) => {
  for (const fieldError of fieldErrors ?? []) {
    if (!fieldError.field || !fieldError.message) {
      continue;
    }

    switch (fieldError.field) {
      case "text":
      case "foregroundCharacter":
      case "foregroundColor":
      case "backgroundCharacter":
      case "backgroundColor":
      case "type":
        setError(fieldError.field, {
          type: "server",
          message: fieldError.message,
        });
        break;
      default:
        break;
    }
  }
};
