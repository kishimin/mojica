import { useState } from "react";
import { Controller } from "react-hook-form";
import { usePostImages } from "@/api/endpoints/image/image";
import ColorPickerField from "@/components/ColorPickerField/ColorPickerField";
import TextField from "@/components/TextField/TextField";
import {
  imageGenerationErrorMessages,
  imageGenerationFormMessages,
  imageGenerationValidationMessages,
} from "@/i18n/messages";
import AlertBanner from "@/components/AlertBanner/AlertBanner";
import { toImageGenerationErrorPresentation } from "../../utils/toImageGenerationErrorPresentation";
import { useImageGenerationForm } from "../../hooks/useImageGenerationForm";
import GenerateButton from "../GenerateButton/GenerateButton";
import ImageTypeSelect from "../ImageTypeSelect/ImageTypeSelect";
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
  const {
    register,
    control,
    handleSubmit,
    setError,
    formState: { errors, isSubmitting },
  } = useImageGenerationForm();
  const { isPending, mutate } = usePostImages({
    mutation: {
      onError: (error) => {
        const response = error.response?.data;
        const fieldErrors =
          response && "errors" in response ? response.errors : undefined;

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

        if ((fieldErrors ?? []).length === 0) {
          const presentation = toImageGenerationErrorPresentation(
            response?.code,
          );
          setApiError({
            title: imageGenerationErrorMessages[locale][presentation],
            description: response?.message ?? "",
          });
        }
      },
    },
  });
  const messages = imageGenerationFormMessages[locale];

  const submitForm = handleSubmit((values) => {
    setApiError(undefined);
    mutate({ data: values });
  });

  const getErrorMessage = (message: string | undefined) =>
    message
      ? (imageGenerationValidationMessages[locale][message] ?? message)
      : undefined;

  return (
    <form onSubmit={submitForm}>
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
        name="foregroundColor"
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
        name="backgroundColor"
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
        name="type"
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
          isPending || isSubmitting ? { kind: "submitting" } : { kind: "idle" }
        }
      />
    </form>
  );
};

export default ImageGenerationForm;
