type FieldErrorProps = {
  id?: string;
  message?: string;
};

const FieldError = ({ id, message }: FieldErrorProps) => {
  if (!message) {
    return null;
  }

  return <p id={id}>{message}</p>;
};

export default FieldError;
