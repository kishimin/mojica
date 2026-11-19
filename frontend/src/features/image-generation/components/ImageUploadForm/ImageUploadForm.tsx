import type { AxiosResponse } from "axios";
import { useEffect, useRef, useState } from "react";
import type { DragEvent, FormEvent } from "react";
import { useRetryAfterCountdown } from "../../hooks/useRetryAfterCountdown";
import { imageGenerationSchema } from "../../schemas/imageGenerationSchema";
import { downloadGeneratedImage } from "../../utils/downloadGeneratedImage";
import { parseImageGenerationErrorResponse } from "../../utils/parseImageGenerationErrorResponse";
import { toImageGenerationApiError } from "../../utils/toImageGenerationApiError";
import { toRetryAfterSeconds } from "../../utils/toRetryAfterSeconds";
import { usePostImagesFrameFile } from "@/api/endpoints/image-upload/image-upload";
import AlertBanner from "@/components/AlertBanner/AlertBanner";
import ColorPickerField from "@/components/ColorPickerField/ColorPickerField";
import TextField from "@/components/TextField/TextField";
import { Button } from "@/components/ui/button";
import {
  generateButtonMessages,
  imageGenerationFormMessages,
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
    const parsed = imageGenerationSchema.safeParse({ text: "x", type: "standard", ...values });
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
            setFieldErrors(serverErrors);
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
    <form onChange={invalidatePreview} onSubmit={handleSubmit}>
      {apiError ? <AlertBanner title={apiError.title} description={apiError.description} /> : null}
      <label htmlFor={"frame-image"}>{messages.sourceImage}</label>
      <input
        id={"frame-image"}
        type={"file"}
        accept={"image/png,image/jpeg,image/webp"}
        aria-describedby={"frame-image-help"}
        onDragOver={(event) => event.preventDefault()}
        onDrop={handleDrop}
        onChange={(event) => { setFile(event.target.files?.[0]); setFileError(false); }}
      />
      {file ? <p>{file.name}</p> : null}
      <p id={"frame-image-help"}>{messages.imageHelp}</p>
      {fileError ? <p role={"alert"}>{messages.imageRequired}</p> : null}
      <TextField label={messages.pattern} errorMessage={fieldErrors.foregroundCharacter} value={pattern} onChange={(event) => setPattern(event.target.value)} />
      <ColorPickerField label={messages.patternColor} colorPickerLabel={messages.patternColorPicker} value={patternColor} errorMessage={fieldErrors.foregroundColor} onChange={setPatternColor} />
      <TextField label={messages.background} errorMessage={fieldErrors.backgroundCharacter} value={background} onChange={(event) => setBackground(event.target.value)} />
      <ColorPickerField label={messages.backgroundColor} colorPickerLabel={messages.backgroundColorPicker} value={backgroundColor} errorMessage={fieldErrors.backgroundColor} onChange={setBackgroundColor} />
      <Button type={"submit"} disabled={upload.isPending || remainingRetryAfterSeconds > 0}>
        {remainingRetryAfterSeconds > 0 ? generateButtonMessages[locale].cooldown(remainingRetryAfterSeconds) : generateButtonMessages[locale].idle}
      </Button>
      {preview ? <div>
        <img src={preview.url} alt={imageGenerationFormMessages[locale].previewAlt} />
        <Button type={"button"} onClick={() => downloadGeneratedImage(preview.response)}>{imageGenerationFormMessages[locale].download}</Button>
      </div> : <div>
        <p>{imageGenerationFormMessages[locale].previewPlaceholder}</p>
        <Button type={"button"} disabled>{imageGenerationFormMessages[locale].download}</Button>
      </div>}
    </form>
  );
};

export default ImageUploadForm;
