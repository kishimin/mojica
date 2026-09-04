type FieldErrorProps = {
  /** Optional id referenced by the invalid form control. */
  id?: string;
  /** Validation message; no element is rendered when omitted. */
  message?: string;
};

const FieldError = ({ id, message }: FieldErrorProps) => {
  if (!message) {
    return null;
  }

  return (
    <p id={id} aria-live={"polite"} className={"text-xs text-destructive"}>
      {message}
    </p>
  );
};

export default FieldError;
