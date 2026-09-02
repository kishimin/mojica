import { act, renderHook } from "@testing-library/react";
import { afterEach, beforeEach, describe, expect, test, vi } from "vitest";
import { useRetryAfterCountdown } from "./useRetryAfterCountdown";

describe("useRetryAfterCountdown", () => {
  beforeEach(() => {
    vi.useFakeTimers();
  });

  afterEach(() => {
    vi.useRealTimers();
  });

  describe("initial value", () => {
    test("starts with the supplied Retry-After duration", () => {
      const { result } = renderHook(() => useRetryAfterCountdown(5));

      expect(result.current).toBe(5);
    });
  });

  describe("countdown", () => {
    test("decreases the remaining seconds once per elapsed second", () => {
      const { result } = renderHook(() => useRetryAfterCountdown(5));

      act(() => {
        vi.advanceTimersByTime(1000);
      });

      expect(result.current).toBe(4);
    });

    test("stops decrementing after reaching zero", () => {
      const { result } = renderHook(() => useRetryAfterCountdown(1));

      act(() => {
        vi.advanceTimersByTime(3000);
      });

      expect(result.current).toBe(0);
    });
  });

  describe("input changes", () => {
    test("restarts from the new duration when Retry-After changes", () => {
      const { result, rerender } = renderHook(
        ({ seconds }: { seconds: number }) => useRetryAfterCountdown(seconds),
        { initialProps: { seconds: 5 } },
      );

      act(() => {
        vi.advanceTimersByTime(2000);
      });
      expect(result.current).toBe(3);

      rerender({ seconds: 10 });

      expect(result.current).toBe(10);
    });
  });

  describe("lifecycle", () => {
    test("disposes the countdown timer when unmounted", () => {
      const { unmount } = renderHook(() => useRetryAfterCountdown(5));

      expect(vi.getTimerCount()).toBe(1);

      unmount();

      expect(vi.getTimerCount()).toBe(0);
    });
  });
});
