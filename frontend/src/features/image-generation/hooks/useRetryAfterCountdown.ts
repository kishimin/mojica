import { useEffect, useState } from "react";

/** Provides the module's public behavior. */
export const useRetryAfterCountdown = (retryAfterSeconds: number) => {
  const [remainingSeconds, setRemainingSeconds] = useState(retryAfterSeconds);

  useEffect(() => {
    setRemainingSeconds(retryAfterSeconds);

    if (retryAfterSeconds <= 0) {
      return;
    }

    // Do not recalculate the duration on each tick; a fixed deadline preserves elapsed time.
    const deadline = Date.now() + retryAfterSeconds * 1000;
    const timerId = setInterval(() => {
      const nextRemainingSeconds = Math.max(
        0,
        Math.ceil((deadline - Date.now()) / 1000),
      );

      setRemainingSeconds(nextRemainingSeconds);

      if (nextRemainingSeconds === 0) {
        clearInterval(timerId);
      }
    }, 1000);

    return () => clearInterval(timerId);
  }, [retryAfterSeconds]);

  return remainingSeconds;
};
