import { act, renderHook } from "@testing-library/react";
import { beforeEach, describe, expect, test } from "vitest";
import { useI18n } from "../../hooks/use-i18n";
import { I18nProvider } from "./I18nProvider";

describe("I18nProvider locale contract", () => {
  beforeEach(() => {
    localStorage.clear();
  });

  test("rejects consumers outside I18nProvider", () => {
    const renderWithoutProvider = () => renderHook(useI18n);

    expect(renderWithoutProvider).toThrow(
      "useI18n must be used within I18nProvider",
    );
  });

  test("uses Japanese when no locale has been persisted", () => {
    const { result } = renderHook(() => useI18n(), {
      wrapper: I18nProvider,
    });

    expect(result.current.locale).toBe("ja");
  });

  test("restores persisted English as the active locale", () => {
    localStorage.setItem("locale", "en");

    const { result } = renderHook(() => useI18n(), {
      wrapper: I18nProvider,
    });

    expect(result.current.locale).toBe("en");
  });

  test("falls back to Japanese for an unsupported persisted locale", () => {
    localStorage.setItem("locale", "fr");

    const { result } = renderHook(() => useI18n(), {
      wrapper: I18nProvider,
    });

    expect(result.current.locale).toBe("ja");
  });

  test("updates and persists a supported locale selected by a consumer", () => {
    const { result } = renderHook(() => useI18n(), {
      wrapper: I18nProvider,
    });

    act(() => result.current.setLocale("en"));

    expect(result.current.locale).toBe("en");
    expect(localStorage.getItem("locale")).toBe("en");
  });
});
