import { useEffect, useState } from "react";

export const useRetryAfterCountdown = (retryAfterSeconds: number) => {
  const [remainingSeconds, setRemainingSeconds] = useState(retryAfterSeconds);

  useEffect(() => {
    setRemainingSeconds(retryAfterSeconds);

    if (retryAfterSeconds <= 0) {
      return;
    }

    const timerId = setInterval(() => {
      setRemainingSeconds((currentSeconds) => Math.max(0, currentSeconds - 1));
    }, 1000);

    return () => clearInterval(timerId);
  }, [retryAfterSeconds]);

  return remainingSeconds;
};
