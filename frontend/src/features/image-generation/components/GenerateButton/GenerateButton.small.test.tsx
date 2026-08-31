import { screen } from "@testing-library/react";
import { describe, expect, test } from "vitest";
import GenerateButton from "./GenerateButton";
import { setup } from "@/tests/test-utils";

describe("GenerateButton", () => {
  test("displays an enabled generate action while idle", () => {
    setup(<GenerateButton state={{ kind: "idle" }} />);

    const button = screen.getByRole("button", { name: "画像を生成する" });

    expect(button).toBeEnabled();
    expect(button).not.toHaveAttribute("aria-busy", "true");
  });

  test("communicates the disabled busy state while submitting", () => {
    setup(<GenerateButton state={{ kind: "submitting" }} />);

    const button = screen.getByRole("button", { name: "生成中..." });

    expect(button).toBeDisabled();
    expect(button).toHaveAttribute("aria-busy", "true");
  });

  test("displays an enabled retryable action after an error", () => {
    setup(<GenerateButton state={{ kind: "retryable" }} />);

    const button = screen.getByRole("button", { name: "画像を生成する" });

    expect(button).toBeEnabled();
    expect(button).toHaveClass("bg-inverse");
    expect(button).not.toHaveAttribute("aria-busy", "true");
  });

  test("displays the disabled retry countdown without owning time passage", () => {
    setup(<GenerateButton state={{ kind: "cooldown", remainingSeconds: 5 }} />);

    const button = screen.getByRole("button", {
      name: "5秒後に再試行できます",
    });

    expect(button).toBeDisabled();
    expect(button).not.toHaveAttribute("aria-busy", "true");
  });
});
