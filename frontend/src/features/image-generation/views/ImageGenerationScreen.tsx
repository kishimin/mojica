import { useState } from "react";
import ImageGenerationForm from "../components/ImageGenerationForm/ImageGenerationForm";
import Paper from "@/components/Paper/Paper";
import { useI18n } from "@/hooks/use-i18n";
import { imageGenerationScreenMessages } from "@/i18n/messages";

/** Renders the localized image-generation page body. */
const ImageGenerationScreen = () => {
  const { locale } = useI18n();
  const messages = imageGenerationScreenMessages[locale];
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
        <fieldset>
          <legend>{locale === "ja" ? "作成方法" : "Creation method"}</legend>
          <label><input type={"radio"} name={"mode"} checked={mode === "text"} onChange={() => setMode("text")} />{locale === "ja" ? "文字を文字で描く" : "Draw text with text"}</label>
          <label><input type={"radio"} name={"mode"} checked={mode === "image"} onChange={() => setMode("image")} />{locale === "ja" ? "画像を文字で描く" : "Draw images with text"}</label>
        </fieldset>
        <Paper
          className={
            "w-full min-w-0 px-[var(--layout-form-card-padding-inline)] py-[var(--layout-form-card-padding-block)]"
          }
        >
          {mode === "text" ? <ImageGenerationForm locale={locale} /> : <form><label htmlFor={"frame-image"}>{locale === "ja" ? "元画像" : "Source image"}</label><input id={"frame-image"} type={"file"} /></form>}
        </Paper>
      </div>
    </main>
  );
};

export default ImageGenerationScreen;
