import { useId } from "react";
import type { ChangeEvent, ComponentPropsWithoutRef } from "react";
import FieldError from "@/components/FieldError/FieldError";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { cn } from "@/lib/utils";

type ColorPickerFieldProps = Omit<
  ComponentPropsWithoutRef<"input">,
  "type" | "value" | "onChange"
> & {
  /** Visible label for the combined color controls. */
  label: string;
  /** Accessible name for the native color picker control. */
  colorPickerLabel: string;
  /** Current HEX color value. */
  value: string;
  /** Called when either color control produces a new HEX value. */
  onChange: (hex: string) => void;
  /** Validation message shown beneath the combined control. */
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
        role={"group"}
        aria-label={label}
        className={cn(
          "flex h-14 items-center gap-3 rounded-lg border px-2.5",
          errorMessage
            ? "border-destructive focus-within:ring-3 focus-within:ring-destructive/20"
            : "border-input focus-within:border-ring focus-within:ring-3 focus-within:ring-ring/50",
        )}
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
          className={
            "h-auto border-0 bg-transparent p-0 shadow-none focus-visible:ring-0 disabled:bg-transparent aria-invalid:border-0 aria-invalid:ring-0"
          }
          value={value}
          onChange={handleChange}
        />
      </div>
      <FieldError id={`${inputId}-error`} message={errorMessage} />
    </div>
  );
};

export default ColorPickerField;
