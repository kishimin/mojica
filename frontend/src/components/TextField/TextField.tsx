import { useId } from "react";
import type { ComponentPropsWithoutRef } from "react";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";

type TextFieldProps = ComponentPropsWithoutRef<"input"> & {
  label: string;
};

const TextField = ({ label, id, ...inputProps }: TextFieldProps) => {
  const generatedId = useId();
  const inputId = id ?? generatedId;

  return (
    <div>
      <Label htmlFor={inputId}>{label}</Label>
      <Input id={inputId} {...inputProps} />
    </div>
  );
};

export default TextField;
