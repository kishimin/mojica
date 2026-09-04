import { Controller } from "react-hook-form";
import { usePostImages } from "@/api/endpoints/image/image";
import ColorPickerField from "@/components/ColorPickerField/ColorPickerField";
import TextField from "@/components/TextField/TextField";
import { Button } from "@/components/ui/button";
import {
  imageGenerationFormMessages,
  imageGenerationValidationMessages,
} from "@/i18n/messages";
import { useImageGenerationForm } from "../../hooks/useImageGenerationForm";
import GenerateButton from "../GenerateButton/GenerateButton";
import ImageTypeSelect from "../ImageTypeSelect/ImageTypeSelect";
import type { Locale } from "@/types/i18n";

type ImageGenerationFormProps = {
  /** Locale used for this form's labels. */
  locale: Locale;
};

/** Renders the image-generation inputs and their initial values. */
const ImageGenerationForm = ({ locale }: ImageGenerationFormProps) => {
  const {
    register,
    control,
    handleSubmit,
    formState: { errors, isSubmitting },
  } = useImageGenerationForm();
  const { isPending, mutate } = usePostImages();
  const messages = imageGenerationFormMessages[locale];

  const submitForm = handleSubmit((values) => {
    mutate({ data: values });
  });

  const getErrorMessage = (message: string | undefined) =>
    message ? imageGenerationValidationMessages[locale][message] : undefined;

  return (
    <form onSubmit={submitForm}>
      <TextField
        label={messages.text}
        errorMessage={getErrorMessage(errors.text?.message)}
        {...register("text")}
      />
      <TextField
        label={messages.foregroundCharacter}
        errorMessage={getErrorMessage(errors.foregroundCharacter?.message)}
        {...register("foregroundCharacter")}
      />
      <Controller
        name="foregroundColor"
        control={control}
        render={({ field }) => (
          <ColorPickerField
            label={messages.foregroundColor}
            colorPickerLabel={messages.foregroundColorPicker}
            value={field.value}
            onChange={field.onChange}
            errorMessage={getErrorMessage(errors.foregroundColor?.message)}
          />
        )}
      />
      <TextField
        label={messages.backgroundCharacter}
        errorMessage={getErrorMessage(errors.backgroundCharacter?.message)}
        {...register("backgroundCharacter")}
      />
      <Controller
        name="backgroundColor"
        control={control}
        render={({ field }) => (
          <ColorPickerField
            label={messages.backgroundColor}
            colorPickerLabel={messages.backgroundColorPicker}
            value={field.value}
            onChange={field.onChange}
            errorMessage={getErrorMessage(errors.backgroundColor?.message)}
          />
        )}
      />
      <Controller
        name="type"
        control={control}
        render={({ field }) => (
          <ImageTypeSelect
            value={field.value}
            onChange={field.onChange}
            errorMessage={getErrorMessage(errors.type?.message)}
          />
        )}
      />
      <GenerateButton
        state={
          isPending || isSubmitting ? { kind: "submitting" } : { kind: "idle" }
        }
      />
    </form>
  );
};

export default ImageGenerationForm;
