import { Controller } from "react-hook-form";
import { usePostImages } from "@/api/endpoints/image/image";
import ColorPickerField from "@/components/ColorPickerField/ColorPickerField";
import TextField from "@/components/TextField/TextField";
import { Button } from "@/components/ui/button";
import {
  generateButtonMessages,
  imageGenerationFormMessages,
} from "@/i18n/messages";
import { useImageGenerationForm } from "../../hooks/useImageGenerationForm";
import ImageTypeSelect from "../ImageTypeSelect/ImageTypeSelect";
import type { Locale } from "@/types/i18n";

type ImageGenerationFormProps = {
  /** Locale used for this form's labels. */
  locale: Locale;
};

/** Renders the image-generation inputs and their initial values. */
const ImageGenerationForm = ({ locale }: ImageGenerationFormProps) => {
  const { register, control, handleSubmit } = useImageGenerationForm();
  const { mutate } = usePostImages();
  const messages = imageGenerationFormMessages[locale];

  const submitForm = handleSubmit((values) => {
    mutate({ data: values });
  });

  return (
    <form onSubmit={submitForm}>
      <TextField label={messages.text} {...register("text")} />
      <TextField
        label={messages.foregroundCharacter}
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
          />
        )}
      />
      <TextField
        label={messages.backgroundCharacter}
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
          />
        )}
      />
      <Controller
        name="type"
        control={control}
        render={({ field }) => (
          <ImageTypeSelect value={field.value} onChange={field.onChange} />
        )}
      />
      <Button type="submit">{generateButtonMessages[locale].idle}</Button>
    </form>
  );
};

export default ImageGenerationForm;
