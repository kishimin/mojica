import { type ClassValue, clsx } from "clsx"
import { twMerge } from "tailwind-merge"

/**
 * Merges class names, resolving Tailwind class conflicts in favor of the later one.
 */
export function cn(...inputs: ClassValue[]) {
  return twMerge(clsx(inputs))
}
