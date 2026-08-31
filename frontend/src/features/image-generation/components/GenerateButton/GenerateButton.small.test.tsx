import { screen } from "@testing-library/react";
import { describe, expect, test } from "vitest";

import { setup } from "@/tests/test-utils";

import GenerateButton from "./GenerateButton";

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

  // ID: GENERATE-BUTTON-S-003
  // Source: docs/v1/ui/components/GenerateButton.md § Display by state, § Tests
  // Given: The button state is retryable
  // When: The button is rendered
  // Then: It displays an enabled generate action with the retryable appearance
  // Blocked by: GenerateButton implementation
  // Priority: P1
  test.todo("displays an enabled retryable action after an error");

  // ID: GENERATE-BUTTON-S-004
  // Source: docs/v1/ui/components/GenerateButton.md § Display by state, § Tests
  // Given: The button state is cooldown with a remaining-second value
  // When: The button is rendered
  // Then: It displays the remaining seconds and is disabled without being busy
  // Blocked by: GenerateButton implementation
  // Priority: P0
  test.todo("displays the disabled retry countdown without owning time passage");
});
