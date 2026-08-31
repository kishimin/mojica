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
  "aria-errormessage": externalErrorId,
  "aria-invalid": isExternallyInvalid,
  ...inputProps
}: TextFieldProps) => {
  const generatedId = useId();
  const inputId = id ?? generatedId;
  const errorId = `${inputId}-error`;

  return (
    <div className={"flex flex-col gap-2"}>
      <Label htmlFor={inputId}>{label}</Label>
      <Input
        id={inputId}
        aria-describedby={describedBy}
        aria-errormessage={errorMessage ? errorId : externalErrorId}
        aria-invalid={errorMessage ? true : isExternallyInvalid}
        {...inputProps}
      />
      <FieldError id={errorId} message={errorMessage} />
    </div>
  );
};

export default TextField;
