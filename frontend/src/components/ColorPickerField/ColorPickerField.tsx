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
  colorPickerLabel: string;
  value: string;
  onChange: (hex: string) => void;
  errorMessage?: string;
};

const ColorPickerField = ({
  label,
  colorPickerLabel,
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
    <div className={"flex flex-col gap-2"}>
      <Label htmlFor={inputId}>{label}</Label>
      <div
        className={
          "flex h-14 items-center gap-3 rounded-lg border border-input px-2.5 focus-within:border-ring focus-within:ring-3 focus-within:ring-ring/50"
        }
      >
        <input
          id={colorPickerId}
          type={"color"}
          aria-label={colorPickerLabel}
          aria-describedby={describedBy}
          aria-errormessage={
            errorMessage ? `${inputId}-error` : externalErrorId
          }
          aria-invalid={errorMessage ? true : isExternallyInvalid}
          disabled={inputProps.disabled}
          className={"size-9 shrink-0 rounded-md border-0 p-0"}
          value={value}
          onChange={handleChange}
        />
        <Input
          {...inputProps}
          id={inputId}
          aria-describedby={describedBy}
          aria-errormessage={
            errorMessage ? `${inputId}-error` : externalErrorId
          }
          aria-invalid={errorMessage ? true : isExternallyInvalid}
          className={"h-auto border-0 p-0 shadow-none focus-visible:ring-0"}
          value={value}
          onChange={handleChange}
        />
      </div>
      <FieldError id={`${inputId}-error`} message={errorMessage} />
    </div>
  );
};

export default ColorPickerField;
