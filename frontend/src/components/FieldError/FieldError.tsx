type FieldErrorProps = {
  message?: string;
};

const FieldError = ({ message }: FieldErrorProps) => {
  if (!message) {
    return null;
  }

  return <p>{message}</p>;
};

export default FieldError;
