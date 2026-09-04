import { describe, expect, test } from "vitest";
import toGenerateButtonState from "./toGenerateButtonState";

describe("toGenerateButtonState", () => {
  test("prioritizes submitting while a request is being sent", () => {
    expect(
      toGenerateButtonState({
        isPending: true,
        isSubmitting: true,
        remainingRetryAfterSeconds: 5,
        hasApiError: true,
      }),
    ).toEqual({ kind: "submitting" });
  });

  test("returns cooldown while retry is blocked", () => {
    expect(
      toGenerateButtonState({
        isPending: false,
        isSubmitting: false,
        remainingRetryAfterSeconds: 5,
        hasApiError: true,
      }),
    ).toEqual({ kind: "cooldown", remainingSeconds: 5 });
  });

  test("returns retryable after an API error when no cooldown remains", () => {
    expect(
      toGenerateButtonState({
        isPending: false,
        isSubmitting: false,
        remainingRetryAfterSeconds: 0,
        hasApiError: true,
      }),
    ).toEqual({ kind: "retryable" });
  });

  test("returns idle when no request or error is active", () => {
    expect(
      toGenerateButtonState({
        isPending: false,
        isSubmitting: false,
        remainingRetryAfterSeconds: 0,
        hasApiError: false,
      }),
    ).toEqual({ kind: "idle" });
  });
});
