import { Controller } from "react-hook-form";
import { useImageGenerationController } from "../../hooks/useImageGenerationController";
import { downloadGeneratedImage } from "../../utils/downloadGeneratedImage";
import { toGenerateButtonState } from "../../utils/toGenerateButtonState";
import GenerateButton from "../GenerateButton/GenerateButton";
import ImageTypeSelect from "../ImageTypeSelect/ImageTypeSelect";
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
  const {
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
  } = useImageGenerationController(locale);
  const messages = imageGenerationFormMessages[locale];
  const screenMessages = imageGenerationScreenMessages[locale];

  const getErrorMessage = (message: string | undefined) =>
    message
      ? (imageGenerationValidationMessages[locale][message] ?? message)
      : undefined;

  return (
    <form
      className={"min-w-0 flex flex-col gap-6"}
      onChange={invalidatePreview}
      onSubmit={onSubmit}
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
