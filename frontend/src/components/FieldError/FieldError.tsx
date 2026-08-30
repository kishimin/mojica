type FieldErrorProps = {
  message?: string;
  id?: string;
};

const FieldError = ({ id, message }: FieldErrorProps) => {
  if (!message) {
    return null;
  }

  return <p id={id}>{message}</p>;
};

export default FieldError;
