/** UI states exposed by the image-generation action button. */
export type GenerateButtonState =
  | { kind: "idle" }
  | { kind: "submitting" }
  | { kind: "retryable" }
  | { kind: "cooldown"; remainingSeconds: number };
