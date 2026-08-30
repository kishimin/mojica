import { useId, type ComponentPropsWithoutRef } from "react";

import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";

import FieldError from "../FieldError/FieldError";

type TextFieldProps = ComponentPropsWithoutRef<"input"> & {
  label: string;
  errorMessage?: string;
};

const TextField = ({
  "aria-describedby": ariaDescribedBy,
  errorMessage,
  id: providedId,
  label,
  ...inputProps
}: TextFieldProps) => {
  const generatedId = useId();
  const inputId = providedId ?? `text-field-${generatedId}`;
  const errorId = `${inputId}-error`;
  const describedBy = [ariaDescribedBy, errorMessage ? errorId : undefined]
    .filter(Boolean)
    .join(" ");

  return (
    <div>
      <Label htmlFor={inputId}>{label}</Label>
      <Input
        {...inputProps}
        id={inputId}
        aria-describedby={describedBy || undefined}
        aria-invalid={errorMessage ? true : inputProps["aria-invalid"]}
      />
      <FieldError id={errorMessage ? errorId : undefined} message={errorMessage} />
    </div>
  );
};

export default TextField;
