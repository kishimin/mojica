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

const imageTypeOptions = [
  { value: "standard" },
  { value: "x-background" },
  { value: "x-icon" },
] as const satisfies readonly { value: ImageType }[];

const ImageTypeSelect = ({
  value,
  onChange,
  errorMessage,
}: ImageTypeSelectProps) => {
  const { locale } = useI18n();
  const messages = imageTypeSelectMessages[locale];
  const errorId = "image-type-select-error";

  return (
    <div className={"flex flex-col gap-2"}>
      <Label htmlFor={"image-type-select"}>{messages.label}</Label>
      <Select value={value} onValueChange={onChange}>
        <SelectTrigger
          id={"image-type-select"}
          aria-describedby={errorMessage ? errorId : undefined}
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
