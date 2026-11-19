import { useState } from "react";
import { Button } from "@/components/ui/button";
import { generateButtonMessages, imageModeMessages } from "@/i18n/messages";
import type { Locale } from "@/types/i18n";

type Props = { locale: Locale };

/** Collects the inputs for image-based generation. */
const ImageUploadForm = ({ locale }: Props) => {
  const [file, setFile] = useState<File>();
  const [fileError, setFileError] = useState(false);
  const messages = imageModeMessages[locale];

  return (
    <form onSubmit={(event) => { event.preventDefault(); setFileError(!file); }}>
      <label htmlFor={"frame-image"}>{messages.sourceImage}</label>
      <input id={"frame-image"} type={"file"} onChange={(event) => { setFile(event.target.files?.[0]); setFileError(false); }} />
      {fileError ? <p role={"alert"}>{messages.imageRequired}</p> : null}
      <Button type={"submit"}>{generateButtonMessages[locale].idle}</Button>
    </form>
  );
};

export default ImageUploadForm;
