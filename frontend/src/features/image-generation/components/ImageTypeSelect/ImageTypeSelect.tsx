import { useId } from "react";
import { imageTypeOptions } from "./image-type-options";
import FieldError from "@/components/FieldError/FieldError";
import { Label } from "@/components/ui/label";
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select";
import { useI18n } from "@/hooks/use-i18n";
import { imageTypeSelectMessages } from "@/i18n/messages";
import type { ImageType } from "@/types/image-type";

type ImageTypeSelectProps = {
  /** The image type controlled by the parent form. */
  value: ImageType;
  /** Called when the user selects another image type. */
  onChange: (value: ImageType) => void;
  /** Optional validation message associated with the selector. */
  errorMessage?: string;
};

/** Provides the module's public behavior. */
const ImageTypeSelect = ({
  value,
  onChange,
  errorMessage,
}: ImageTypeSelectProps) => {
  const { locale } = useI18n();
  const messages = imageTypeSelectMessages[locale];
  const generatedId = useId();
  const selectId = `image-type-select-${generatedId}`;
  const errorId = `${selectId}-error`;

  return (
    <div className={"flex flex-col gap-2"}>
      <Label htmlFor={selectId}>{messages.label}</Label>
      <Select value={value} onValueChange={onChange}>
        <SelectTrigger
          id={selectId}
          aria-errormessage={errorMessage ? errorId : undefined}
          aria-invalid={errorMessage ? true : undefined}
          className={"w-full"}
        >
          <SelectValue />
        </SelectTrigger>
        <SelectContent>
          {imageTypeOptions.map((option) => (
            <SelectItem key={option.value} value={option.value}>
              {messages.options[option.value]}
            </SelectItem>
          ))}
        </SelectContent>
      </Select>
      <FieldError id={errorId} message={errorMessage} />
    </div>
  );
};

export default ImageTypeSelect;
