import { useMutation } from "@tanstack/react-query";
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
import { customInstance } from "@/api/mutator/custom-instance";
import { imageGenerationFormMessages } from "@/i18n/messages";
import type { Locale } from "@/types/i18n";

/** Coordinates image generation and invalidates results for outdated inputs. */
export const useImageGenerationController = (locale: Locale) => {
  const [preview, setPreview] = useState<{ response: AxiosResponse<Blob>; url: string }>();
  const [mode, setMode] = useState<"text" | "image">("text");
  const [file, setFile] = useState<File>();
  const [fileError, setFileError] = useState(false);
  const revision = useRef(0);
  useEffect(() => () => {
    if (preview) URL.revokeObjectURL(preview.url);
  }, [preview]);
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
    getValues,
    trigger,
    setError,
    formState: { errors, isSubmitting },
  } = useImageGenerationForm();
  const { isPending, mutate } = usePostImages<PostImagesMutationError>();
  const messages = imageGenerationFormMessages[locale];
  const upload = useMutation<AxiosResponse<Blob>, PostImagesMutationError, FormData>({
    mutationFn: (data) => customInstance<Blob>({ url: "/images/frame-file", method: "POST", data, responseType: "blob" }),
  });
  const invalidatePreview = () => {
    revision.current += 1;
    setPreview(undefined);
  };
  const handleApiError = async (error: PostImagesMutationError, submittedRevision: number) => {
    const response = await parseImageGenerationErrorResponse(error.response?.data);
    if (submittedRevision !== revision.current) return;
    const fieldErrors = response && "errors" in response ? response.errors : undefined;
    applyImageGenerationFieldErrors(fieldErrors, setError);
    if ((fieldErrors ?? []).length === 0) {
      const presentation = toImageGenerationApiError(response, locale);
      setApiError(mode === "image" && response?.code === "IMAGE_OUTPUT_SIZE_EXCEEDED"
        ? { ...presentation, description: messages.imageOutputTooLarge }
        : presentation);
      setRetryAfterSeconds(toRetryAfterSeconds(error.response?.headers));
    }
  };

  const submitImage = async () => {
    const submittedRevision = revision.current;
    if (!file) {
      setFileError(true);
      return;
    }
    const valid = await trigger(["foregroundCharacter", "foregroundColor", "backgroundCharacter", "backgroundColor"]);
    if (!valid || submittedRevision !== revision.current) return;
    setApiError(undefined);
    setRetryAfterSeconds(0);
    const values = getValues();
    const data = new FormData();
    data.append("frameImage", file);
    data.append("foregroundCharacter", values.foregroundCharacter);
    data.append("foregroundColor", values.foregroundColor);
    data.append("backgroundCharacter", values.backgroundCharacter);
    data.append("backgroundColor", values.backgroundColor);
    upload.mutate(data, {
      onSuccess: (response) => {
        if (submittedRevision === revision.current) setPreview({ response, url: URL.createObjectURL(response.data) });
      },
      onError: (error) => { void handleApiError(error, submittedRevision); },
    });
  };

  const submitForm = (submittedRevision: number) => handleSubmit((values) => {
    if (submittedRevision !== revision.current) return;
    setApiError(undefined);
    setRetryAfterSeconds(0);
    mutate({ data: values }, {
      onSuccess: (response) => {
        if (submittedRevision !== revision.current) return;
        setPreview({ response, url: URL.createObjectURL(response.data) });
      },
      onError: (error) => { void handleApiError(error, submittedRevision); },
    });
  });

  const onSubmit: FormEventHandler<HTMLFormElement> = (event) => {
    if (mode === "text") void submitForm(revision.current)(event);
    else { event.preventDefault(); void submitImage(); }
  };
  return { register, control, errors, isPending: isPending || upload.isPending, isSubmitting, apiError, remainingRetryAfterSeconds, preview, invalidatePreview, onSubmit, mode, setMode, file, setFile, fileError, setFileError, setApiError };
};

export type ImageGenerationController = ReturnType<typeof useImageGenerationController>;
