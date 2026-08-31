import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select";
import FieldError from "@/components/FieldError/FieldError";
import type { ImageGenerationRequestDto } from "@/models/imageGenerationRequestDto";
import { Label } from "@/components/ui/label";

type ImageType = NonNullable<ImageGenerationRequestDto["type"]>;

type ImageTypeSelectProps = {
  /** The image type controlled by the parent form. */
  value: ImageType;
  /** Called when the user selects another image type. */
  onChange: (value: ImageType) => void;
  /** Optional validation message associated with the selector. */
  errorMessage?: string;
};

const imageTypeOptions = [
  { value: "standard", label: "標準画像" },
  { value: "x-background", label: "X背景画像" },
  { value: "x-icon", label: "Xアイコン画像" },
] as const satisfies readonly { value: ImageType; label: string }[];

const ImageTypeSelect = ({
  value,
  onChange,
  errorMessage,
}: ImageTypeSelectProps) => {
  const errorId = "image-type-select-error";

  return (
    <div className={"flex flex-col gap-2"}>
      <Label htmlFor="image-type-select">画像タイプ</Label>
      <Select value={value} onValueChange={onChange}>
        <SelectTrigger
          id="image-type-select"
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
              {option.label}
            </SelectItem>
          ))}
        </SelectContent>
      </Select>
      <FieldError id={errorId} message={errorMessage} />
    </div>
  );
};

export default ImageTypeSelect;
