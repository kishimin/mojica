import { useState } from "react";

export const useRetryAfterCountdown = (retryAfterSeconds: number) =>
  useState(retryAfterSeconds)[0];
