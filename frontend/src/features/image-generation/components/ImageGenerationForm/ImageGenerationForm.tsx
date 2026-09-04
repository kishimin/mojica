import { Controller } from "react-hook-form";
import ColorPickerField from "@/components/ColorPickerField/ColorPickerField";
import TextField from "@/components/TextField/TextField";
import { Button } from "@/components/ui/button";
import { imageGenerationFormMessages } from "@/i18n/messages";
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
  const messages = imageGenerationFormMessages[locale];

  return (
    <form onSubmit={handleSubmit(() => undefined)}>
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
            colorPickerLabel={`${messages.foregroundColor}を選択`}
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
            colorPickerLabel={`${messages.backgroundColor}を選択`}
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
      <Button type="submit">画像を生成する</Button>
    </form>
  );
};

export default ImageGenerationForm;
