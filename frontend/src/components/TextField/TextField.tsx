import { useId } from "react";
import type { ComponentPropsWithoutRef } from "react";
import FieldError from "@/components/FieldError/FieldError";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { cn } from "@/lib/utils";

type TextFieldProps = ComponentPropsWithoutRef<"input"> & {
  /** Visible label associated with the textbox. */
  label: string;
  /** Validation message announced for the textbox. */
  errorMessage?: string;
  /** Supporting guidance displayed below the textbox. */
  helperText?: string;
};

const TextField = ({
  label,
  id,
  errorMessage,
  helperText,
  "aria-describedby": describedBy,
  "aria-errormessage": externalErrorId,
  "aria-invalid": isExternallyInvalid,
  className,
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
        className={cn("disabled:bg-transparent", className)}
        {...inputProps}
      />
      {helperText ? (
        <p className={"text-xs text-helper-foreground"}>{helperText}</p>
      ) : null}
      <FieldError id={errorId} message={errorMessage} />
    </div>
  );
};

export default TextField;
