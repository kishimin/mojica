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
  "aria-describedby": describedBy,
  "aria-errormessage": callerErrorMessageId,
  "aria-invalid": callerInvalid,
  ...inputProps
}: TextFieldProps) => {
  const generatedId = useId();
  const inputId = id ?? generatedId;
  const errorId = `${inputId}-error`;

  return (
    <div>
      <Label htmlFor={inputId}>{label}</Label>
      <Input
        id={inputId}
        aria-describedby={describedBy}
        aria-errormessage={errorMessage ? errorId : callerErrorMessageId}
        aria-invalid={errorMessage ? true : callerInvalid}
        {...inputProps}
      />
      <FieldError id={errorId} message={errorMessage} />
    </div>
  );
};

export default TextField;
