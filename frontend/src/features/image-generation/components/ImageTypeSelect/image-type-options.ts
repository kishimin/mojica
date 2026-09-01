import type { ImageType } from "@/types/image-type";

export const imageTypeOptions = [
  { value: "standard" },
  { value: "x-background" },
  { value: "x-icon" },
] as const satisfies readonly { value: ImageType }[];
