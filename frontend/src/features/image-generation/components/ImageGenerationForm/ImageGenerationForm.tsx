import { useState } from "react";
import { Controller } from "react-hook-form";
import { useImageGenerationForm } from "../../hooks/useImageGenerationForm";
import { useRetryAfterCountdown } from "../../hooks/useRetryAfterCountdown";
import applyImageGenerationFieldErrors from "../../utils/applyImageGenerationFieldErrors";
import toImageGenerationApiError from "../../utils/toImageGenerationApiError";
import toRetryAfterSeconds from "../../utils/toRetryAfterSeconds";
import GenerateButton from "../GenerateButton/GenerateButton";
import ImageTypeSelect from "../ImageTypeSelect/ImageTypeSelect";
import {
  usePostImages,
  type PostImagesMutationError,
} from "@/api/endpoints/image/image";
import AlertBanner from "@/components/AlertBanner/AlertBanner";
import ColorPickerField from "@/components/ColorPickerField/ColorPickerField";
import TextField from "@/components/TextField/TextField";
import {
  imageGenerationFormMessages,
  imageGenerationValidationMessages,
} from "@/i18n/messages";
import type { Locale } from "@/types/i18n";

type ImageGenerationFormProps = {
  /** Locale used for this form's labels. */
  locale: Locale;
};

/** Renders the image-generation inputs and their initial values. */
const ImageGenerationForm = ({ locale }: ImageGenerationFormProps) => {
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
  const { isPending, mutate } = usePostImages<PostImagesMutationError>({
    mutation: {
      onError: (error) => {
        const response = error.response?.data;
        const fieldErrors =
          response && "errors" in response ? response.errors : undefined;

        applyImageGenerationFieldErrors(fieldErrors, setError);

        if ((fieldErrors ?? []).length === 0) {
          setApiError(toImageGenerationApiError(response, locale));
          setRetryAfterSeconds(toRetryAfterSeconds(error.response?.headers));
        }
      },
    },
  });
  const messages = imageGenerationFormMessages[locale];

  const submitForm = handleSubmit((values) => {
    setApiError(undefined);
    setRetryAfterSeconds(0);
    mutate({ data: values });
  });

  const getErrorMessage = (message: string | undefined) =>
    message
      ? (imageGenerationValidationMessages[locale][message] ?? message)
      : undefined;

  return (
    <form
      onSubmit={(event) => {
        void submitForm(event);
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
        errorMessage={getErrorMessage(errors.text?.message)}
        {...register("text")}
      />
      <TextField
        label={messages.foregroundCharacter}
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
            onChange={field.onChange}
            errorMessage={getErrorMessage(errors.type?.message)}
          />
        )}
      />
      <GenerateButton
        state={
          isPending || isSubmitting
            ? { kind: "submitting" }
            : remainingRetryAfterSeconds > 0
              ? {
                  kind: "cooldown",
                  remainingSeconds: remainingRetryAfterSeconds,
                }
              : apiError
                ? { kind: "retryable" }
                : { kind: "idle" }
        }
      />
    </form>
  );
};

export default ImageGenerationForm;
