import type { AxiosResponse } from "axios";
import { useEffect, useRef, useState } from "react";
import type { FormEventHandler } from "react";
import { applyImageGenerationFieldErrors } from "../utils/applyImageGenerationFieldErrors";
import { parseImageGenerationErrorResponse } from "../utils/parseImageGenerationErrorResponse";
import { toImageGenerationApiError } from "../utils/toImageGenerationApiError";
import { toRetryAfterSeconds } from "../utils/toRetryAfterSeconds";
import { useImageGenerationForm } from "./useImageGenerationForm";
import { useRetryAfterCountdown } from "./useRetryAfterCountdown";
import {
  usePostImages,
  type PostImagesMutationError,
} from "@/api/endpoints/image/image";
import type { Locale } from "@/types/i18n";

/** Coordinates image generation and invalidates results for outdated inputs. */
export const useImageGenerationController = (locale: Locale) => {
  const [preview, setPreview] = useState<{
    response: AxiosResponse<Blob>;
    url: string;
  }>();
  const revision = useRef(0);
  useEffect(
    () => () => {
      if (preview) URL.revokeObjectURL(preview.url);
    },
    [preview],
  );
  const [apiError, setApiError] = useState<{
    title: string;
    description: string;
  }>();
  const [retryAfterSeconds, setRetryAfterSeconds] = useState(0);
  const remainingRetryAfterSeconds = useRetryAfterCountdown(retryAfterSeconds);
  const {
    register,
    control,
    handleSubmit,
    setError,
    formState: { errors, isSubmitting },
  } = useImageGenerationForm();
  const { isPending, mutate } = usePostImages<PostImagesMutationError>();
  const invalidatePreview = () => {
    revision.current += 1;
    setPreview(undefined);
  };
  const handleApiError = async (
    error: PostImagesMutationError,
    submittedRevision: number,
  ) => {
    const response = await parseImageGenerationErrorResponse(
      error.response?.data,
    );
    if (submittedRevision !== revision.current) return;
    const fieldErrors =
      response && "errors" in response ? response.errors : undefined;
    applyImageGenerationFieldErrors(fieldErrors, setError);
    if ((fieldErrors ?? []).length === 0) {
      setApiError(toImageGenerationApiError(response, locale));
      setRetryAfterSeconds(toRetryAfterSeconds(error.response?.headers));
    }
  };

  const submitForm = (submittedRevision: number) =>
    handleSubmit((values) => {
      if (submittedRevision !== revision.current) return;
      setApiError(undefined);
      setRetryAfterSeconds(0);
      mutate({ data: values }, {
        onSuccess: (response) => {
          if (submittedRevision !== revision.current) return;
          setPreview({ response, url: URL.createObjectURL(response.data) });
        },
        onError: (error) => {
          void handleApiError(error, submittedRevision);
        },
      });
    });

  const onSubmit: FormEventHandler<HTMLFormElement> = (event) => {
    void submitForm(revision.current)(event);
  };

  return {
    register,
    control,
    errors,
    isPending,
    isSubmitting,
    apiError,
    remainingRetryAfterSeconds,
    preview,
    invalidatePreview,
    onSubmit,
  };
};
