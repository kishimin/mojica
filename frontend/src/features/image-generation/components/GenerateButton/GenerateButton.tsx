import { Button } from "@/components/ui/button";
import { Loader2 } from "lucide-react";

type GenerateButtonState =
  | { kind: "idle" }
  | { kind: "submitting" }
  | { kind: "retryable" }
  | { kind: "cooldown"; remainingSeconds: number };

type GenerateButtonProps = {
  state: GenerateButtonState;
};

function GenerateButton({ state }: GenerateButtonProps) {
  const isSubmitting = state.kind === "submitting";

  return (
    <Button disabled={isSubmitting} aria-busy={isSubmitting}>
      {isSubmitting ? <Loader2 aria-hidden={true} className="animate-spin" /> : null}
      {isSubmitting ? "生成中..." : "画像を生成する"}
    </Button>
  );
}

export default GenerateButton;
