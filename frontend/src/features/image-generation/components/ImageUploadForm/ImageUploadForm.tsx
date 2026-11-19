import type { AxiosResponse } from "axios";
import { useEffect, useRef, useState } from "react";
import { parseImageGenerationErrorResponse } from "../../utils/parseImageGenerationErrorResponse";
import { toImageGenerationApiError } from "../../utils/toImageGenerationApiError";
import { downloadGeneratedImage } from "../../utils/downloadGeneratedImage";
import { usePostImagesFrameFile } from "@/api/endpoints/image-upload/image-upload";
import AlertBanner from "@/components/AlertBanner/AlertBanner";
import ColorPickerField from "@/components/ColorPickerField/ColorPickerField";
import TextField from "@/components/TextField/TextField";
import { Button } from "@/components/ui/button";
import { generateButtonMessages, imageGenerationFormMessages, imageModeMessages } from "@/i18n/messages";
import type { Locale } from "@/types/i18n";

type Props = { locale: Locale };

/** Collects the inputs for image-based generation. */
const ImageUploadForm = ({ locale }: Props) => {
  const revision = useRef(0);
  const [preview, setPreview] = useState<{ response: AxiosResponse<Blob>; url: string }>();
  useEffect(() => () => { if (preview) URL.revokeObjectURL(preview.url); }, [preview]);
  const [file, setFile] = useState<File>();
  const [fileError, setFileError] = useState(false);
  const [apiError, setApiError] = useState<{ title: string; description: string }>();
  const [pattern, setPattern] = useState("");
  const [background, setBackground] = useState("");
  const [patternColor, setPatternColor] = useState("#000000");
  const [backgroundColor, setBackgroundColor] = useState("#ffffff");
  const upload = usePostImagesFrameFile();
  const messages = imageModeMessages[locale];

  return (
    <form onChange={() => { revision.current += 1; setPreview(undefined); }} onSubmit={(event) => { event.preventDefault(); const submittedRevision = revision.current; setApiError(undefined); setFileError(!file); if (file) upload.mutate({ data: { frameImage: file, foregroundCharacter: pattern, backgroundCharacter: background, foregroundColor: patternColor, backgroundColor } }, {
        onSuccess: (response) => { if (submittedRevision === revision.current) setPreview({ response, url: URL.createObjectURL(response.data) }); },
        onError: (error) => { void (async () => {
          const response = await parseImageGenerationErrorResponse(error.response?.data);
          if (submittedRevision === revision.current) setApiError(toImageGenerationApiError(response, locale));
        })(); },
      }); }}>
      {apiError ? <AlertBanner title={apiError.title} description={apiError.description} /> : null}
      <label htmlFor={"frame-image"}>{messages.sourceImage}</label>
      <input id={"frame-image"} type={"file"} onChange={(event) => { setFile(event.target.files?.[0]); setFileError(false); }} />
      {fileError ? <p role={"alert"}>{messages.imageRequired}</p> : null}
      <TextField label={messages.pattern} value={pattern} onChange={(event) => setPattern(event.target.value)} />
      <ColorPickerField label={messages.patternColor} colorPickerLabel={messages.patternColorPicker} value={patternColor} onChange={setPatternColor} />
      <TextField label={messages.background} value={background} onChange={(event) => setBackground(event.target.value)} />
      <ColorPickerField label={messages.backgroundColor} colorPickerLabel={messages.backgroundColorPicker} value={backgroundColor} onChange={setBackgroundColor} />
      <Button type={"submit"} disabled={upload.isPending}>{generateButtonMessages[locale].idle}</Button>
      {preview ? <div>
        <img src={preview.url} alt={imageGenerationFormMessages[locale].previewAlt} />
        <Button type={"button"} onClick={() => downloadGeneratedImage(preview.response)}>{imageGenerationFormMessages[locale].download}</Button>
      </div> : null}
    </form>
  );
};

export default ImageUploadForm;
