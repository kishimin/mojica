import { useMutation } from "@tanstack/react-query";
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
import { customInstance } from "@/api/mutator/custom-instance";
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
  const screenMessages = imageGenerationScreenMessages[locale];
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

  const getErrorMessage = (message: string | undefined) =>
    message
      ? (imageGenerationValidationMessages[locale][message] ?? message)
      : undefined;

  return (
    <form
      className={"min-w-0 flex flex-col gap-6"}
      onChange={invalidatePreview}
      onSubmit={(event) => {
        if (mode === "text") void submitForm(revision.current)(event);
        else {
          event.preventDefault();
          void submitImage();
        }
      }}
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
          isPending: isPending || upload.isPending,
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
