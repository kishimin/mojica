import { useId } from "react";
import type { ChangeEvent, ComponentPropsWithoutRef } from "react";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";

type ColorPickerFieldProps = Omit<
  ComponentPropsWithoutRef<"input">,
  "type" | "value" | "onChange"
> & {
  label: string;
  value: string;
  onChange: (hex: string) => void;
  errorMessage?: string;
};

const ColorPickerField = ({
  label,
  value,
  onChange,
  errorMessage: _errorMessage,
  id,
  ...inputProps
}: ColorPickerFieldProps) => {
  const generatedId = useId();
  const inputId = id ?? generatedId;
  const colorPickerId = `${inputId}-picker`;

  const handleChange = (event: ChangeEvent<HTMLInputElement>) => {
    onChange(event.target.value);
  };

  return (
    <div>
      <Label htmlFor={inputId}>{label}</Label>
      <Input
        {...inputProps}
        id={inputId}
        value={value}
        onChange={handleChange}
      />
      <input
        id={colorPickerId}
        type={"color"}
        aria-label={`${label} picker`}
        disabled={inputProps.disabled}
        value={value}
        onChange={handleChange}
      />
    </div>
  );
};

export default ColorPickerField;
