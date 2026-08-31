import { screen } from "@testing-library/react";
import { afterEach, describe, expect, test } from "vitest";
import GenerateButton from "./GenerateButton";
import { setupWithI18n } from "@/tests/test-utils";

describe("GenerateButton", () => {
  afterEach(() => {
    localStorage.removeItem("locale");
  });

  test("displays an enabled generate action while idle", () => {
    setupWithI18n(<GenerateButton state={{ kind: "idle" }} />);

    const button = screen.getByRole("button", { name: "画像を生成する" });

    expect(button).toBeEnabled();
  });

  test("communicates the disabled busy state while submitting", () => {
    setupWithI18n(<GenerateButton state={{ kind: "submitting" }} />);

    const button = screen.getByRole("button", { name: "生成中..." });

    expect(button).toBeDisabled();
    expect(button).toHaveAttribute("aria-busy", "true");
  });

  test("displays an enabled retryable action after an error", () => {
    setupWithI18n(<GenerateButton state={{ kind: "retryable" }} />);

    const button = screen.getByRole("button", { name: "画像を生成する" });

    expect(button).toBeEnabled();
  });

  test("displays the disabled retry countdown without owning time passage", () => {
    setupWithI18n(
      <GenerateButton state={{ kind: "cooldown", remainingSeconds: 5 }} />,
    );

    const button = screen.getByRole("button", {
      name: "5秒後に再試行できます",
    });

    expect(button).toBeDisabled();
  });

  test("displays the English label when English is the active locale", () => {
    localStorage.setItem("locale", "en");

    setupWithI18n(<GenerateButton state={{ kind: "submitting" }} />, "en");

    const button = screen.getByRole("button", { name: "Generating..." });

    expect(button).toBeDisabled();
  });
});
