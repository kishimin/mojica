import { useId } from "react";
import type { ComponentPropsWithoutRef } from "react";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import FieldError from "@/components/FieldError/FieldError";

type TextFieldProps = ComponentPropsWithoutRef<"input"> & {
  label: string;
  errorMessage?: string;
};

const TextField = ({ label, id, errorMessage, ...inputProps }: TextFieldProps) => {
  const generatedId = useId();
  const inputId = id ?? generatedId;
  const errorId = `${inputId}-error`;

  return (
    <div>
      <Label htmlFor={inputId}>{label}</Label>
      <Input id={inputId} aria-describedby={errorMessage ? errorId : undefined} {...inputProps} />
      <FieldError id={errorId} message={errorMessage} />
    </div>
  );
};

export default TextField;
