import { useId } from "react";
import type { ComponentPropsWithoutRef } from "react";
import FieldError from "@/components/FieldError/FieldError";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";

type TextFieldProps = ComponentPropsWithoutRef<"input"> & {
  label: string;
  errorMessage?: string;
};

const TextField = ({
  label,
  id,
  errorMessage,
  "aria-describedby": callerDescriptionIds,
  ...inputProps
}: TextFieldProps) => {
  const generatedId = useId();
  const inputId = id ?? generatedId;
  const errorId = `${inputId}-error`;
  const descriptionIds = [callerDescriptionIds, errorMessage ? errorId : undefined]
    .filter(Boolean)
    .join(" ");

  return (
    <div>
      <Label htmlFor={inputId}>{label}</Label>
      <Input id={inputId} aria-describedby={descriptionIds || undefined} {...inputProps} />
      <FieldError id={errorId} message={errorMessage} />
    </div>
  );
};

export default TextField;
