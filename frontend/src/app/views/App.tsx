import AppFooter from "../components/AppFooter/AppFooter";
import AppHeader from "../components/AppHeader/AppHeader";
import { AppProviders } from "../providers/AppProviders";
import ImageGenerationForm from "@/features/image-generation/components/ImageGenerationForm/ImageGenerationForm";
import { useI18n } from "@/hooks/use-i18n";
import { imageGenerationScreenMessages } from "@/i18n/messages";

const ImageGenerationScreen = () => {
  const { locale } = useI18n();
  const messages = imageGenerationScreenMessages[locale];

  return (
    <main className={"flex-1 px-4 pt-8 md:px-6 md:pt-12"}>
      <div className={"mx-auto flex w-full max-w-[620px] flex-col gap-7"}>
        <section className={"flex flex-col items-center gap-3 text-center"}>
          <h1 className={"text-2xl font-semibold"}>{messages.heading}</h1>
          <p className={"text-sm text-muted-foreground"}>
            {messages.description}
          </p>
        </section>
        <ImageGenerationForm locale={locale} />
      </div>
    </main>
  );
};

const App = () => (
  <AppProviders>
    <div className={"flex min-h-screen flex-col"}>
      <AppHeader />
      <ImageGenerationScreen />
      <AppFooter />
    </div>
  </AppProviders>
);

export default App;
