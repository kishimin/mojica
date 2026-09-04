import type { GenerateButtonState } from "@/types/generate-button-state";

type GenerateButtonStateInputs = {
  isPending: boolean;
  isSubmitting: boolean;
  remainingRetryAfterSeconds: number;
  hasApiError: boolean;
};

const toGenerateButtonState = ({
  isPending,
  isSubmitting,
  remainingRetryAfterSeconds,
  hasApiError,
}: GenerateButtonStateInputs): GenerateButtonState => {
  if (isPending || isSubmitting) {
    return { kind: "submitting" };
  }

  if (remainingRetryAfterSeconds > 0) {
    return {
      kind: "cooldown",
      remainingSeconds: remainingRetryAfterSeconds,
    };
  }

  if (hasApiError) {
    return { kind: "retryable" };
  }

  return { kind: "idle" };
};

export default toGenerateButtonState;
