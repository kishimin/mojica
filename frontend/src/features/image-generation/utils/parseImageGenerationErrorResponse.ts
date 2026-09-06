import type {
  ApiErrorResponse,
  ApiValidationErrorResponse,
  ApiValidationFieldError,
} from "@/models";

export type ImageGenerationErrorResponse =
  | ApiErrorResponse
  | ApiValidationErrorResponse;

const isRecord = (value: unknown): value is Record<string, unknown> =>
  typeof value === "object" && value !== null;

const isFieldError = (value: unknown): value is ApiValidationFieldError => {
  if (!isRecord(value)) {
    return false;
  }

  return (
    (typeof value.field === "string" || value.field === null) &&
    (typeof value.message === "string" || value.message === null)
  );
};

const parseJson = (value: string): unknown => {
  try {
    return JSON.parse(value) as unknown;
  } catch {
    return undefined;
  }
};

const parseResponse = (
  value: unknown,
): ImageGenerationErrorResponse | undefined => {
  if (!isRecord(value)) {
    return undefined;
  }

  const response: ApiValidationErrorResponse = {
    code:
      typeof value.code === "string" || value.code === null
        ? value.code
        : undefined,
    message:
      typeof value.message === "string" || value.message === null
        ? value.message
        : undefined,
    errors: Array.isArray(value.errors)
      ? value.errors.filter(isFieldError)
      : undefined,
  };

  return response;
};

/** Provides the module's public behavior. */
export const parseImageGenerationErrorResponse = async (
  value: unknown,
): Promise<ImageGenerationErrorResponse | undefined> => {
  if (value instanceof Blob) {
    return parseResponse(parseJson(await value.text()));
  }

  return parseResponse(value);
};
