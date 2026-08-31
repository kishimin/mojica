import { Button } from "@/components/ui/button";

type GenerateButtonState =
  | { kind: "idle" }
  | { kind: "submitting" }
  | { kind: "retryable" }
  | { kind: "cooldown"; remainingSeconds: number };

type GenerateButtonProps = {
  state: GenerateButtonState;
};

function GenerateButton({ state }: GenerateButtonProps) {
  return (
    <Button disabled={false} aria-busy={false}>
      {state.kind === "idle" ? "画像を生成する" : "画像を生成する"}
    </Button>
  );
}

export default GenerateButton;
