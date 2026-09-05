import ImageGenerationForm from "../components/ImageGenerationForm/ImageGenerationForm";
import Paper from "@/components/Paper/Paper";
import { useI18n } from "@/hooks/use-i18n";
import { imageGenerationScreenMessages } from "@/i18n/messages";

/** Renders the localized image-generation page body. */
const ImageGenerationScreen = () => {
  const { locale } = useI18n();
  const messages = imageGenerationScreenMessages[locale];

  return (
    <main className={"min-w-0 flex-1 px-0 pt-8 md:px-6 md:pt-12"}>
      <div className={"mx-auto flex w-full max-w-[620px] flex-col gap-7"}>
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
        <Paper
          className={
            "w-screen max-w-none min-w-0 px-[var(--layout-form-card-padding-inline)] py-[var(--layout-form-card-padding-block)] md:w-auto md:max-w-full"
          }
        >
          <ImageGenerationForm locale={locale} />
        </Paper>
      </div>
    </main>
  );
};

export default ImageGenerationScreen;
