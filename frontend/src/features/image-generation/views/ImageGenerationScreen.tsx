import { useState } from "react";
import ImageGenerationForm from "../components/ImageGenerationForm/ImageGenerationForm";
import ImageUploadForm from "../components/ImageUploadForm/ImageUploadForm";
import Paper from "@/components/Paper/Paper";
import { useI18n } from "@/hooks/use-i18n";
import { imageGenerationScreenMessages, imageModeMessages } from "@/i18n/messages";

/** Renders the localized image-generation page body. */
const ImageGenerationScreen = () => {
  const { locale } = useI18n();
  const messages = imageGenerationScreenMessages[locale];
  const modeMessages = imageModeMessages[locale];
  const [mode, setMode] = useState<"text" | "image">("text");

  return (
    <main className={"min-w-0 flex-1 px-0 pt-8 md:px-6 md:pt-12"}>
      <div
        className={
          "mx-auto flex w-full max-w-none flex-col gap-7 md:max-w-[620px]"
        }
      >
        <section
          className={
            "flex flex-col items-center gap-3 px-4 text-center md:px-0"
          }
        >
          <h1 className={"text-2xl font-semibold"}>{messages.heading}</h1>
          <p className={"text-sm text-muted-foreground"}>
            {messages.description}
          </p>
        </section>
        <fieldset className={"flex rounded-lg bg-muted p-1"}>
          <legend className={"sr-only"}>{modeMessages.mode}</legend>
          <label className={"relative flex-1 cursor-pointer rounded-md p-3 text-center has-checked:bg-primary has-checked:text-primary-foreground focus-within:ring-3 focus-within:ring-ring/50"}>
            <input className={"absolute inset-0 h-full w-full cursor-pointer opacity-0"} type={"radio"} name={"mode"} checked={mode === "text"} onChange={() => setMode("text")} />
            {modeMessages.textMode}
          </label>
          <label className={"relative flex-1 cursor-pointer rounded-md p-3 text-center has-checked:bg-primary has-checked:text-primary-foreground focus-within:ring-3 focus-within:ring-ring/50"}>
            <input className={"absolute inset-0 h-full w-full cursor-pointer opacity-0"} type={"radio"} name={"mode"} checked={mode === "image"} onChange={() => setMode("image")} />
            {modeMessages.imageMode}
          </label>
        </fieldset>
        <Paper
          className={
            "w-full min-w-0 px-[var(--layout-form-card-padding-inline)] py-[var(--layout-form-card-padding-block)]"
          }
        >
          {mode === "text" ? <ImageGenerationForm locale={locale} /> : <ImageUploadForm locale={locale} />}
        </Paper>
      </div>
    </main>
  );
};

export default ImageGenerationScreen;
