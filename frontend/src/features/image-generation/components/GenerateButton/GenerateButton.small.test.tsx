import { screen } from "@testing-library/react";
import { afterEach, describe, expect, test } from "vitest";
import GenerateButton from "./GenerateButton";
import { I18nContext } from "@/hooks/i18n-context";
import { setup } from "@/tests/test-utils";

describe("GenerateButton", () => {
  afterEach(() => {
    localStorage.removeItem("locale");
  });

  test("displays an enabled generate action while idle", () => {
    setup(
      <I18nContext.Provider
        value={{ locale: "ja", setLocale: () => undefined }}
      >
        <GenerateButton state={{ kind: "idle" }} />
      </I18nContext.Provider>,
    );

    const button = screen.getByRole("button", { name: "画像を生成する" });

    expect(button).toBeEnabled();
    expect(button).not.toHaveAttribute("aria-busy", "true");
  });

  test("communicates the disabled busy state while submitting", () => {
    setup(
      <I18nContext.Provider
        value={{ locale: "ja", setLocale: () => undefined }}
      >
        <GenerateButton state={{ kind: "submitting" }} />
      </I18nContext.Provider>,
    );

    const button = screen.getByRole("button", { name: "生成中..." });

    expect(button).toBeDisabled();
    expect(button).toHaveAttribute("aria-busy", "true");
  });

  test("displays an enabled retryable action after an error", () => {
    setup(
      <I18nContext.Provider
        value={{ locale: "ja", setLocale: () => undefined }}
      >
        <GenerateButton state={{ kind: "retryable" }} />
      </I18nContext.Provider>,
    );

    const button = screen.getByRole("button", { name: "画像を生成する" });

    expect(button).toBeEnabled();
    expect(button).toHaveClass("bg-inverse");
    expect(button).not.toHaveAttribute("aria-busy", "true");
  });

  test("displays the disabled retry countdown without owning time passage", () => {
    setup(
      <I18nContext.Provider
        value={{ locale: "ja", setLocale: () => undefined }}
      >
        <GenerateButton state={{ kind: "cooldown", remainingSeconds: 5 }} />
      </I18nContext.Provider>,
    );

    const button = screen.getByRole("button", {
      name: "5秒後に再試行できます",
    });

    expect(button).toBeDisabled();
    expect(button).not.toHaveAttribute("aria-busy", "true");
  });

  test("displays the English label when English is the active locale", () => {
    localStorage.setItem("locale", "en");

    setup(
      <I18nContext.Provider
        value={{ locale: "en", setLocale: () => undefined }}
      >
        <GenerateButton state={{ kind: "submitting" }} />
      </I18nContext.Provider>,
    );

    expect(
      screen.getByRole("button", { name: "Generating..." }),
    ).toBeDisabled();
  });
});
