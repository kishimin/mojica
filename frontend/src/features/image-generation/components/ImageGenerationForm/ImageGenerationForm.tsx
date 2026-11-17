import type { AxiosResponse } from "axios";
import { useEffect, useRef, useState } from "react";
import { Controller } from "react-hook-form";
import { useImageGenerationForm } from "../../hooks/useImageGenerationForm";
import { useRetryAfterCountdown } from "../../hooks/useRetryAfterCountdown";
import { applyImageGenerationFieldErrors } from "../../utils/applyImageGenerationFieldErrors";
import { downloadGeneratedImage } from "../../utils/downloadGeneratedImage";
import { parseImageGenerationErrorResponse } from "../../utils/parseImageGenerationErrorResponse";
import { toGenerateButtonState } from "../../utils/toGenerateButtonState";
import { toImageGenerationApiError } from "../../utils/toImageGenerationApiError";
import { toRetryAfterSeconds } from "../../utils/toRetryAfterSeconds";
import GenerateButton from "../GenerateButton/GenerateButton";
import ImageTypeSelect from "../ImageTypeSelect/ImageTypeSelect";
import {
  usePostImages,
  type PostImagesMutationError,
} from "@/api/endpoints/image/image";
import AlertBanner from "@/components/AlertBanner/AlertBanner";
import ColorPickerField from "@/components/ColorPickerField/ColorPickerField";
import TextField from "@/components/TextField/TextField";
import { Button } from "@/components/ui/button";
import {
  imageGenerationFormMessages,
  imageGenerationScreenMessages,
  imageGenerationValidationMessages,
} from "@/i18n/messages";
import type { Locale } from "@/types/i18n";

type ImageGenerationFormProps = {
  /** Locale used for this form's labels. */
  locale: Locale;
};

/** Renders the image-generation inputs and their initial values. */
const ImageGenerationForm = ({ locale }: ImageGenerationFormProps) => {
  const [preview, setPreview] = useState<{ response: AxiosResponse<Blob>; url: string }>();
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
    setError,
    formState: { errors, isSubmitting },
  } = useImageGenerationForm();
  const { isPending, mutate } = usePostImages<PostImagesMutationError>();
  const messages = imageGenerationFormMessages[locale];
  const screenMessages = imageGenerationScreenMessages[locale];
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
      setApiError(toImageGenerationApiError(response, locale));
      setRetryAfterSeconds(toRetryAfterSeconds(error.response?.headers));
    }
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

  const getErrorMessage = (message: string | undefined) =>
    message
      ? (imageGenerationValidationMessages[locale][message] ?? message)
      : undefined;

  return (
    <form
      className={"min-w-0 flex flex-col gap-6"}
      onChange={invalidatePreview}
      onSubmit={(event) => {
        void submitForm(revision.current)(event);
      }}
    >
      {apiError ? (
        <AlertBanner
          title={apiError.title}
          description={apiError.description}
        />
      ) : null}

      <TextField
        label={messages.text}
        helperText={screenMessages.textHelper}
        errorMessage={getErrorMessage(errors.text?.message)}
        {...register("text")}
      />

      <TextField
        label={messages.foregroundCharacter}
        helperText={screenMessages.characterHelper}
        errorMessage={getErrorMessage(errors.foregroundCharacter?.message)}
        {...register("foregroundCharacter")}
      />

      <Controller
        name={"foregroundColor"}
        control={control}
        render={({ field }) => (
          <ColorPickerField
            label={messages.foregroundColor}
            colorPickerLabel={messages.foregroundColorPicker}
            value={field.value}
            onChange={field.onChange}
            errorMessage={getErrorMessage(errors.foregroundColor?.message)}
          />
        )}
      />

      <TextField
        label={messages.backgroundCharacter}
        helperText={screenMessages.characterHelper}
        errorMessage={getErrorMessage(errors.backgroundCharacter?.message)}
        {...register("backgroundCharacter")}
      />

      <Controller
        name={"backgroundColor"}
        control={control}
        render={({ field }) => (
          <ColorPickerField
            label={messages.backgroundColor}
            colorPickerLabel={messages.backgroundColorPicker}
            value={field.value}
            onChange={field.onChange}
            errorMessage={getErrorMessage(errors.backgroundColor?.message)}
          />
        )}
      />

      <Controller
        name={"type"}
        control={control}
        render={({ field }) => (
          <ImageTypeSelect
            value={field.value}
            onChange={(value) => { invalidatePreview(); field.onChange(value); }}
            errorMessage={getErrorMessage(errors.type?.message)}
          />
        )}
      />

      <GenerateButton
        state={toGenerateButtonState({
          isPending,
          isSubmitting,
          remainingRetryAfterSeconds,
          hasApiError: apiError !== undefined,
        })}
      />

      <p className={"text-center text-xs text-helper-foreground"}>
        {screenMessages.downloadHelper}
      </p>
      {preview ? (
        <div className={"flex flex-col items-center gap-4"}>
          <img src={preview.url} alt={messages.previewAlt} className={"max-w-full"} />
          <Button type={"button"} onClick={() => downloadGeneratedImage(preview.response)}>
            {messages.download}
          </Button>
        </div>
      ) : <p>{messages.previewPlaceholder}</p>}
    </form>
  );
};

export default ImageGenerationForm;
