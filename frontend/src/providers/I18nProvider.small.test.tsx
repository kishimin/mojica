import { act, renderHook } from "@testing-library/react";
import { beforeEach, describe, expect, test, vi } from "vitest";
import { useI18n } from "../hooks/use-i18n";
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

  test("falls back to Japanese when locale storage cannot be read", () => {
    const getItemSpy = vi
      .spyOn(Storage.prototype, "getItem")
      .mockImplementation(() => {
        throw new DOMException("blocked", "SecurityError");
      });

    const { result } = renderHook(() => useI18n(), {
      wrapper: I18nProvider,
    });

    expect(result.current.locale).toBe("ja");
    getItemSpy.mockRestore();
  });

  describe("document language synchronization", () => {
    test("sets the document language to the active locale on initial render", () => {
      renderHook(() => useI18n(), { wrapper: I18nProvider });

      expect(document.documentElement.lang).toBe("ja");
    });

    test("updates the document language when a consumer changes locale", () => {
      const { result } = renderHook(() => useI18n(), {
        wrapper: I18nProvider,
      });

      act(() => result.current.setLocale("en"));

      expect(document.documentElement.lang).toBe("en");
    });

    test("restores the previous document language when unmounted", () => {
      document.documentElement.lang = "en-US";

      const { unmount } = renderHook(() => useI18n(), {
        wrapper: I18nProvider,
      });

      expect(document.documentElement.lang).toBe("ja");

      unmount();

      expect(document.documentElement.lang).toBe("en-US");
    });
  });
});
