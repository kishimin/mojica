import { useState } from "react";
import { usePostImagesFrameFile } from "@/api/endpoints/image-upload/image-upload";
import TextField from "@/components/TextField/TextField";
import { Button } from "@/components/ui/button";
import { generateButtonMessages, imageModeMessages } from "@/i18n/messages";
import type { Locale } from "@/types/i18n";

type Props = { locale: Locale };

/** Collects the inputs for image-based generation. */
const ImageUploadForm = ({ locale }: Props) => {
  const [file, setFile] = useState<File>();
  const [fileError, setFileError] = useState(false);
  const [pattern, setPattern] = useState("");
  const [background, setBackground] = useState("");
  const upload = usePostImagesFrameFile();
  const messages = imageModeMessages[locale];

  return (
    <form onSubmit={(event) => { event.preventDefault(); setFileError(!file); if (file) upload.mutate({ data: { frameImage: file, foregroundCharacter: pattern, backgroundCharacter: background, foregroundColor: "#000000", backgroundColor: "#ffffff" } }); }}>
      <label htmlFor={"frame-image"}>{messages.sourceImage}</label>
      <input id={"frame-image"} type={"file"} onChange={(event) => { setFile(event.target.files?.[0]); setFileError(false); }} />
      {fileError ? <p role={"alert"}>{messages.imageRequired}</p> : null}
      <TextField label={messages.pattern} value={pattern} onChange={(event) => setPattern(event.target.value)} />
      <TextField label={messages.background} value={background} onChange={(event) => setBackground(event.target.value)} />
      <Button type={"submit"}>{generateButtonMessages[locale].idle}</Button>
    </form>
  );
};

export default ImageUploadForm;
