import { Button } from "@/components/ui/button";
import { Loader2 } from "lucide-react";

type GenerateButtonState =
  | { kind: "idle" }
  | { kind: "submitting" }
  | { kind: "retryable" }
  | { kind: "cooldown"; remainingSeconds: number };

type GenerateButtonProps = {
  /** Presentation state selected by the image generation form. */
  state: GenerateButtonState;
};

function GenerateButton({ state }: GenerateButtonProps) {
  const isSubmitting = state.kind === "submitting";
  const isCooldown = state.kind === "cooldown";
  const isRetryable = state.kind === "retryable" || isCooldown;
  const label = isSubmitting
    ? "生成中..."
    : isCooldown
      ? `${state.remainingSeconds}秒後に再試行できます`
      : "画像を生成する";

  return (
    <Button
      className={isRetryable ? "bg-inverse text-inverse-foreground" : undefined}
      disabled={isSubmitting || isCooldown}
      aria-busy={isSubmitting}
    >
      {isSubmitting ? (
        <Loader2 aria-hidden={true} className="animate-spin" />
      ) : null}
      {label}
    </Button>
  );
}

export default GenerateButton;
