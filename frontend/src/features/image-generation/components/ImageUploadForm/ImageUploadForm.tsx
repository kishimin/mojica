import { useState } from "react";
import { usePostImagesFrameFile } from "@/api/endpoints/image-upload/image-upload";
import ColorPickerField from "@/components/ColorPickerField/ColorPickerField";
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
  const [patternColor, setPatternColor] = useState("#000000");
  const [backgroundColor, setBackgroundColor] = useState("#ffffff");
  const upload = usePostImagesFrameFile();
  const messages = imageModeMessages[locale];

  return (
    <form onSubmit={(event) => { event.preventDefault(); setFileError(!file); if (file) upload.mutate({ data: { frameImage: file, foregroundCharacter: pattern, backgroundCharacter: background, foregroundColor: patternColor, backgroundColor } }); }}>
      <label htmlFor={"frame-image"}>{messages.sourceImage}</label>
      <input id={"frame-image"} type={"file"} onChange={(event) => { setFile(event.target.files?.[0]); setFileError(false); }} />
      {fileError ? <p role={"alert"}>{messages.imageRequired}</p> : null}
      <TextField label={messages.pattern} value={pattern} onChange={(event) => setPattern(event.target.value)} />
      <ColorPickerField label={messages.patternColor} colorPickerLabel={messages.patternColorPicker} value={patternColor} onChange={setPatternColor} />
      <TextField label={messages.background} value={background} onChange={(event) => setBackground(event.target.value)} />
      <ColorPickerField label={messages.backgroundColor} colorPickerLabel={messages.backgroundColorPicker} value={backgroundColor} onChange={setBackgroundColor} />
      <Button type={"submit"}>{generateButtonMessages[locale].idle}</Button>
    </form>
  );
};

export default ImageUploadForm;
