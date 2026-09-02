import { act, renderHook } from "@testing-library/react";
import { describe, expect, test, vi } from "vitest";
import { useRetryAfterCountdown } from "./useRetryAfterCountdown";

describe("useRetryAfterCountdown", () => {
  describe("initial value", () => {
    // ID: RETRY-AFTER-COUNTDOWN-S-001
    // Source: docs/v1/ui/components/ImageGenerationForm.md § Tests; docs/v1/ui/ui.md § 12
    // Given: The hook receives a positive Retry-After duration in seconds
    // When: The hook is rendered
    // Then: The remaining seconds equal the supplied duration
    // Blocked by: useRetryAfterCountdown implementation
    // Priority: P0
    test("starts with the supplied Retry-After duration", () => {
      const { result } = renderHook(() => useRetryAfterCountdown(5));

      expect(result.current).toBe(5);
    });
  });

  describe("countdown", () => {
    // ID: RETRY-AFTER-COUNTDOWN-S-002
    // Source: docs/v1/ui/components/ImageGenerationForm.md § Tests; docs/v1/ui/ui.md § 12
    // Given: The hook has a positive remaining duration and fake timers are enabled
    // When: One second elapses
    // Then: The remaining seconds decrease by one
    // Blocked by: useRetryAfterCountdown implementation
    // Priority: P0
    test("decreases the remaining seconds once per elapsed second", () => {
      vi.useFakeTimers();

      try {
        const { result } = renderHook(() => useRetryAfterCountdown(5));

        act(() => {
          vi.advanceTimersByTime(1000);
        });

        expect(result.current).toBe(4);
      } finally {
        vi.useRealTimers();
      }
    });

    // ID: RETRY-AFTER-COUNTDOWN-S-003
    // Source: docs/v1/ui/components/ImageGenerationForm.md § Tests; docs/v1/ui/ui.md § 12
    // Given: The hook has one remaining second and fake timers are enabled
    // When: The countdown reaches zero
    // Then: The remaining seconds stay at zero and no further decrement occurs
    // Blocked by: useRetryAfterCountdown implementation
    // Priority: P0
    test("stops decrementing after reaching zero", () => {
      vi.useFakeTimers();

      try {
        const { result } = renderHook(() => useRetryAfterCountdown(1));

        act(() => {
          vi.advanceTimersByTime(3000);
        });

        expect(result.current).toBe(0);
      } finally {
        vi.useRealTimers();
      }
    });
  });

  describe("input changes", () => {
    // ID: RETRY-AFTER-COUNTDOWN-S-004
    // Source: docs/v1/ui/components/ImageGenerationForm.md § Tests; docs/v1/ui/ui.md § 12
    // Given: The hook is counting down from a positive duration
    // When: The supplied Retry-After duration changes
    // Then: The remaining seconds restart from the new duration
    // Blocked by: useRetryAfterCountdown implementation
    // Priority: P0
    test("restarts from the new duration when Retry-After changes", () => {
      vi.useFakeTimers();

      try {
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
      } finally {
        vi.useRealTimers();
      }
    });
  });

  describe("lifecycle", () => {
    // ID: RETRY-AFTER-COUNTDOWN-S-005
    // Source: docs/v1/ui/components/ImageGenerationForm.md § Tests; frontend testing policy
    // Given: The hook has an active countdown timer
    // When: The hook is unmounted
    // Then: The timer is disposed without updating the unmounted hook
    // Blocked by: useRetryAfterCountdown implementation
    // Priority: P1
    test("disposes the countdown timer when unmounted", () => {
      vi.useFakeTimers();

      try {
        const { unmount } = renderHook(() => useRetryAfterCountdown(5));

        expect(vi.getTimerCount()).toBe(1);

        unmount();

        expect(vi.getTimerCount()).toBe(0);
      } finally {
        vi.useRealTimers();
      }
    });
  });
});
