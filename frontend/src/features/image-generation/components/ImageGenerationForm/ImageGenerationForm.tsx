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
  const { register, control, errors, isPending, isSubmitting, apiError, remainingRetryAfterSeconds, preview, invalidatePreview, onSubmit, mode, setMode, setFile, fileError, setFileError, setApiError } = useImageGenerationController(locale);
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

      <fieldset className={"flex gap-4"}>
        <legend>{messages.mode}</legend>
        <label><input type={"radio"} name={"mode"} checked={mode === "text"} onChange={() => { setMode("text"); setFile(undefined); setFileError(false); setApiError(undefined); }} /> {messages.textMode}</label>
        <label><input type={"radio"} name={"mode"} checked={mode === "image"} onChange={() => { setMode("image"); setApiError(undefined); }} /> {messages.imageMode}</label>
      </fieldset>
      {mode === "text" ? <TextField
        label={messages.text}
        helperText={screenMessages.textHelper}
        errorMessage={getErrorMessage(errors.text?.message)}
        {...register("text")}
      /> : <div className={"flex flex-col gap-2"}>
        <label htmlFor={"frame-image"}>{messages.imageFile}</label>
        <input id={"frame-image"} type={"file"} accept={"image/png,image/jpeg,image/webp"} aria-describedby={"frame-image-help"} aria-invalid={fileError} aria-errormessage={fileError ? "frame-image-error" : undefined} onChange={(event) => { setFile(event.target.files?.[0]); setFileError(false); }} />
        <p id={"frame-image-help"}>{messages.imageHelp}</p>
        {fileError ? <p id={"frame-image-error"} role={"alert"}>{messages.imageRequired}</p> : null}
      </div>}

      <TextField
        label={mode === "text" ? messages.foregroundCharacter : messages.imageForeground}
        helperText={screenMessages.characterHelper}
        errorMessage={getErrorMessage(errors.foregroundCharacter?.message)}
        {...register("foregroundCharacter")}
      />

      <Controller
        name={"foregroundColor"}
        control={control}
        render={({ field }) => (
          <ColorPickerField
            label={mode === "text" ? messages.foregroundColor : messages.imageForegroundColor}
            colorPickerLabel={mode === "text" ? messages.foregroundColorPicker : messages.imageForegroundColorPicker}
            value={field.value}
            onChange={field.onChange}
            errorMessage={getErrorMessage(errors.foregroundColor?.message)}
          />
        )}
      />

      <TextField
        label={mode === "text" ? messages.backgroundCharacter : messages.imageBackground}
        helperText={screenMessages.characterHelper}
        errorMessage={getErrorMessage(errors.backgroundCharacter?.message)}
        {...register("backgroundCharacter")}
      />

      <Controller
        name={"backgroundColor"}
        control={control}
        render={({ field }) => (
          <ColorPickerField
            label={mode === "text" ? messages.backgroundColor : messages.imageBackgroundColor}
            colorPickerLabel={mode === "text" ? messages.backgroundColorPicker : messages.imageBackgroundColorPicker}
            value={field.value}
            onChange={field.onChange}
            errorMessage={getErrorMessage(errors.backgroundColor?.message)}
          />
        )}
      />

      {mode === "text" ? <Controller
        name={"type"}
        control={control}
        render={({ field }) => (
          <ImageTypeSelect
            value={field.value}
            onChange={(value) => { invalidatePreview(); field.onChange(value); }}
            errorMessage={getErrorMessage(errors.type?.message)}
          />
        )}
      /> : null}

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

