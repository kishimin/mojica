import { Loader2 } from "lucide-react";
import { Button } from "@/components/ui/button";
import { useI18n } from "@/hooks/use-i18n";
import { generateButtonMessages } from "@/i18n/messages";

type GenerateButtonState =
  | { kind: "idle" }
  | { kind: "submitting" }
  | { kind: "retryable" }
  | { kind: "cooldown"; remainingSeconds: number };

type GenerateButtonProps = {
  /** Presentation state selected by the image generation form. */
  state: GenerateButtonState;
};

type ButtonPresentation = {
  label: string;
  disabled: boolean;
  isSubmitting: boolean;
};

const getButtonPresentation = (
  state: GenerateButtonState,
  locale: keyof typeof generateButtonMessages,
): ButtonPresentation => {
  const messages = generateButtonMessages[locale];

  switch (state.kind) {
    case "idle":
      return {
        label: messages.idle,
        disabled: false,
        isSubmitting: false,
      };
    case "submitting":
      return {
        label: messages.submitting,
        disabled: true,
        isSubmitting: true,
      };
    case "retryable":
      return {
        label: messages.retryable,
        disabled: false,
        isSubmitting: false,
      };
    case "cooldown":
      return {
        label: messages.cooldown(state.remainingSeconds),
        disabled: true,
        isSubmitting: false,
      };
    default:
      return assertNever(state);
  }
};

const assertNever = (value: never): never => {
  throw new Error(`Unsupported generate button state: ${String(value)}`);
};

/** Renders the image generation action for its parent-selected state. */
const GenerateButton = ({ state }: GenerateButtonProps) => {
  const { locale } = useI18n();
  const { label, disabled, isSubmitting } = getButtonPresentation(
    state,
    locale,
  );

  return (
    <Button disabled={disabled} aria-busy={isSubmitting}>
      {isSubmitting ? (
        <Loader2 aria-hidden={true} className={"animate-spin"} />
      ) : null}
      {label}
    </Button>
  );
};

export default GenerateButton;
