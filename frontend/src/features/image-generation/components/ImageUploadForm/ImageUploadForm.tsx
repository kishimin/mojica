import type { AxiosResponse } from "axios";
import { useEffect, useRef, useState } from "react";
import type { DragEvent, FormEvent } from "react";
import { useRetryAfterCountdown } from "../../hooks/useRetryAfterCountdown";
import { imageUploadSchema } from "../../schemas/imageGenerationSchema";
import { downloadGeneratedImage } from "../../utils/downloadGeneratedImage";
import { parseImageGenerationErrorResponse } from "../../utils/parseImageGenerationErrorResponse";
import { toGenerateButtonState } from "../../utils/toGenerateButtonState";
import { toImageGenerationApiError } from "../../utils/toImageGenerationApiError";
import { toRetryAfterSeconds } from "../../utils/toRetryAfterSeconds";
import GenerateButton from "../GenerateButton/GenerateButton";
import { usePostImagesFrameFile } from "@/api/endpoints/image-upload/image-upload";
import uploadIcon from "@/assets/image-upload.svg";
import AlertBanner from "@/components/AlertBanner/AlertBanner";
import ColorPickerField from "@/components/ColorPickerField/ColorPickerField";
import TextField from "@/components/TextField/TextField";
import { Button } from "@/components/ui/button";
import {
  imageGenerationFormMessages,
  imageGenerationScreenMessages,
  imageGenerationValidationMessages,
  imageModeMessages,
} from "@/i18n/messages";
import type { Locale } from "@/types/i18n";

type Props = { locale: Locale };

/** Collects the inputs for image-based generation. */
const ImageUploadForm = ({ locale }: Props) => {
  const revision = useRef(0);
  const [preview, setPreview] = useState<{ response: AxiosResponse<Blob>; url: string }>();
  const [file, setFile] = useState<File>();
  const [fileError, setFileError] = useState(false);
  const [retryAfterSeconds, setRetryAfterSeconds] = useState(0);
  const [apiError, setApiError] = useState<{ title: string; description: string }>();
  const [fieldErrors, setFieldErrors] = useState<Record<string, string>>({});
  const [pattern, setPattern] = useState("");
  const [background, setBackground] = useState("");
  const [patternColor, setPatternColor] = useState("#000000");
  const [backgroundColor, setBackgroundColor] = useState("#ffffff");
  const remainingRetryAfterSeconds = useRetryAfterCountdown(retryAfterSeconds);
  const upload = usePostImagesFrameFile();
  const messages = imageModeMessages[locale];

  useEffect(() => () => { if (preview) URL.revokeObjectURL(preview.url); }, [preview]);

  const invalidatePreview = () => {
    revision.current += 1;
    setPreview(undefined);
  };

  const revalidateField = (name: "foregroundCharacter" | "backgroundCharacter" | "foregroundColor" | "backgroundColor", value: string) => {
    if (!fieldErrors[name]) return;
    const parsed = imageUploadSchema.safeParse({
      foregroundCharacter: name === "foregroundCharacter" ? value : pattern,
      backgroundCharacter: name === "backgroundCharacter" ? value : background,
      foregroundColor: name === "foregroundColor" ? value : patternColor,
      backgroundColor: name === "backgroundColor" ? value : backgroundColor,
    });
    const names = [name];
    if (name === "foregroundCharacter" || name === "backgroundCharacter") {
      const other = name === "foregroundCharacter" ? "backgroundCharacter" : "foregroundCharacter";
      if (fieldErrors[other] === imageGenerationValidationMessages[locale]["characterCombination.required"]) names.push(other);
    }
    setFieldErrors((current) => {
      const next = { ...current };
      for (const field of names) {
        const issue = parsed.success ? undefined : parsed.error.issues.find((item) => item.path[0] === field);
        next[field] = issue ? imageGenerationValidationMessages[locale][issue.message] ?? issue.message : "";
      }
      return next;
    });
  };

  const handleDrop = (event: DragEvent<HTMLInputElement>) => {
    event.preventDefault();
    invalidatePreview();
    setFile(event.dataTransfer.files[0]);
    setFileError(false);
  };

  const handleSubmit = (event: FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    const submittedRevision = revision.current;
    setApiError(undefined);
    setRetryAfterSeconds(0);
    setFileError(!file);
    const values = {
      foregroundCharacter: pattern,
      backgroundCharacter: background,
      foregroundColor: patternColor,
      backgroundColor,
    };
    const parsed = imageUploadSchema.safeParse(values);
    const errors: Record<string, string> = {};
    if (!parsed.success) {
      for (const issue of parsed.error.issues) {
        const name = String(issue.path[0]);
        errors[name] ??= imageGenerationValidationMessages[locale][issue.message] ?? issue.message;
      }
    }
    setFieldErrors(errors);
    if (!file || !parsed.success) return;

    upload.mutate({ data: { frameImage: file, ...values } }, {
      onSuccess: (response) => {
        if (submittedRevision === revision.current) {
          setPreview({ response, url: URL.createObjectURL(response.data) });
        }
      },
      onError: (error) => {
        void (async () => {
          const response = await parseImageGenerationErrorResponse(error.response?.data);
          if (submittedRevision !== revision.current) return;
          const validationErrors = response && "errors" in response ? response.errors : undefined;
          if (validationErrors?.length) {
            const serverErrors: Record<string, string> = {};
            for (const item of validationErrors) {
              if (item.field && item.message && ["foregroundCharacter", "foregroundColor", "backgroundCharacter", "backgroundColor"].includes(item.field)) {
                serverErrors[item.field] = item.message;
              }
            }
            if (Object.keys(serverErrors).length) {
              setFieldErrors(serverErrors);
            } else {
              setApiError(toImageGenerationApiError(response, locale));
            }
          } else {
            const presentation = toImageGenerationApiError(response, locale);
            setRetryAfterSeconds(toRetryAfterSeconds(error.response?.headers));
            setApiError(response?.code === "IMAGE_OUTPUT_SIZE_EXCEEDED"
              ? { ...presentation, description: messages.imageOutputTooLarge }
              : presentation);
          }
        })();
      },
    });
  };

  return (
    <form className={"min-w-0 flex flex-col gap-6"} onChange={invalidatePreview} onSubmit={handleSubmit}>
      {apiError ? <AlertBanner title={apiError.title} description={apiError.description} /> : null}
      <div className={"flex flex-col gap-2"}>
        <span>{messages.sourceImage}</span>
        <label htmlFor={"frame-image"} className={"relative flex min-h-40 cursor-pointer flex-col items-center justify-center gap-2 rounded-lg border border-input bg-muted/30 p-4 text-center focus-within:ring-3 focus-within:ring-ring/50"}>
          <img src={uploadIcon} alt={""} className={"size-8"} />
          <span>{file?.name ?? messages.selectImage}</span>
          <span id={"frame-image-help"} className={"text-xs text-muted-foreground"}>{messages.imageHelp}</span>
          <input
            id={"frame-image"}
            aria-label={messages.sourceImage}
            className={"absolute inset-0 h-full w-full cursor-pointer opacity-0"}
            type={"file"}
            accept={"image/png,image/jpeg,image/webp"}
            aria-describedby={"frame-image-help"}
            aria-invalid={fileError}
            aria-errormessage={fileError ? "frame-image-error" : undefined}
            onDragOver={(event) => event.preventDefault()}
            onDrop={handleDrop}
            onChange={(event) => { setFile(event.target.files?.[0]); setFileError(false); }}
          />
        </label>
        {fileError ? <p id={"frame-image-error"} role={"alert"}>{messages.imageRequired}</p> : null}
      </div>

      <TextField helperText={imageGenerationScreenMessages[locale].characterHelper} label={messages.pattern} errorMessage={fieldErrors.foregroundCharacter} value={pattern} onChange={(event) => { setPattern(event.target.value); revalidateField("foregroundCharacter", event.target.value); }} />

      <ColorPickerField label={messages.patternColor} colorPickerLabel={messages.patternColorPicker} value={patternColor} errorMessage={fieldErrors.foregroundColor} onChange={(value) => { setPatternColor(value); revalidateField("foregroundColor", value); }} />

      <TextField helperText={imageGenerationScreenMessages[locale].characterHelper} label={messages.background} errorMessage={fieldErrors.backgroundCharacter} value={background} onChange={(event) => { setBackground(event.target.value); revalidateField("backgroundCharacter", event.target.value); }} />

      <ColorPickerField label={messages.backgroundColor} colorPickerLabel={messages.backgroundColorPicker} value={backgroundColor} errorMessage={fieldErrors.backgroundColor} onChange={(value) => { setBackgroundColor(value); revalidateField("backgroundColor", value); }} />

      <p className={"text-xs text-helper-foreground"}>{messages.outputType}</p>

      <GenerateButton state={toGenerateButtonState({
        isPending: upload.isPending,
        isSubmitting: false,
        remainingRetryAfterSeconds,
        hasApiError: apiError !== undefined,
      })} />

      <p className={"text-center text-xs text-helper-foreground"}>{imageGenerationScreenMessages[locale].downloadHelper}</p>
      {preview ? <div className={"flex flex-col items-center gap-4"}>
        <img src={preview.url} alt={imageGenerationFormMessages[locale].previewAlt} className={"max-w-full"} />
        <Button type={"button"} onClick={() => downloadGeneratedImage(preview.response)}>{imageGenerationFormMessages[locale].download}</Button>
      </div> : <div className={"flex flex-col gap-4"}>
        <p className={"flex min-h-40 items-center justify-center rounded-lg border border-input bg-muted/30 p-4 text-center text-sm text-muted-foreground"}>{imageGenerationFormMessages[locale].previewPlaceholder}</p>
        <Button type={"button"} disabled>{imageGenerationFormMessages[locale].download}</Button>
      </div>}
    </form>
  );
};

export default ImageUploadForm;
