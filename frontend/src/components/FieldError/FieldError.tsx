type FieldErrorProps = {
  id?: string;
  message?: string;
};

const FieldError = ({ id, message }: FieldErrorProps) => {
  if (!message) {
    return null;
  }

  return (
    <p id={id} className={"text-xs text-destructive"}>
      {message}
    </p>
  );
};

export default FieldError;
