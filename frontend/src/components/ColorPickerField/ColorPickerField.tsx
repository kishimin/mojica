import { useId } from "react";
import type { ChangeEvent, ComponentPropsWithoutRef } from "react";
import FieldError from "@/components/FieldError/FieldError";
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
  errorMessage,
  "aria-describedby": describedBy,
  "aria-errormessage": externalErrorId,
  "aria-invalid": isExternallyInvalid,
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
        aria-describedby={describedBy}
        aria-errormessage={errorMessage ? `${inputId}-error` : externalErrorId}
        aria-invalid={errorMessage ? true : isExternallyInvalid}
        value={value}
        onChange={handleChange}
      />
      <input
        id={colorPickerId}
        type={"color"}
        aria-label={`${label} picker`}
        aria-describedby={describedBy}
        aria-errormessage={errorMessage ? `${inputId}-error` : externalErrorId}
        aria-invalid={errorMessage ? true : isExternallyInvalid}
        disabled={inputProps.disabled}
        value={value}
        onChange={handleChange}
      />
      <FieldError id={`${inputId}-error`} message={errorMessage} />
    </div>
  );
};

export default ColorPickerField;
