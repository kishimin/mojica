import type { ImageGenerationController } from "../../hooks/useImageGenerationController";
import { imageGenerationFormMessages } from "@/i18n/messages";
import type { Locale } from "@/types/i18n";

type Props = { locale: Locale; controller: ImageGenerationController };

/** Selects the input mode for image generation. */
const ImageGenerationModeSelector = ({ locale, controller }: Props) => {
  const messages = imageGenerationFormMessages[locale];
  const { mode, setMode, setFile, setFileError, setApiError, invalidatePreview } = controller;

  return (
    <fieldset className={"flex rounded-lg bg-muted p-1"}>
      <legend className={"sr-only"}>{messages.mode}</legend>
      <label className={"relative flex-1 cursor-pointer rounded-md p-3 text-center has-checked:bg-primary has-checked:text-primary-foreground focus-within:ring-3 focus-within:ring-ring/50"}>
        <input
          className={"absolute inset-0 h-full w-full cursor-pointer opacity-0"}
          type={"radio"}
          name={"mode"}
          checked={mode === "text"}
          onChange={() => {
            invalidatePreview();
            setMode("text");
            setFile(undefined);
            setFileError(false);
            setApiError(undefined);
          }}
        />
        {messages.textMode}
      </label>
      <label className={"relative flex-1 cursor-pointer rounded-md p-3 text-center has-checked:bg-primary has-checked:text-primary-foreground focus-within:ring-3 focus-within:ring-ring/50"}>
        <input
          className={"absolute inset-0 h-full w-full cursor-pointer opacity-0"}
          type={"radio"}
          name={"mode"}
          checked={mode === "image"}
          onChange={() => {
            invalidatePreview();
            setMode("image");
            setApiError(undefined);
          }}
        />
        {messages.imageMode}
      </label>
    </fieldset>
  );
};

export default ImageGenerationModeSelector;
