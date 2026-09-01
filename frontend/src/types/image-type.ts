/** Image type values exposed by the image-generation UI. */
export const imageTypeDefinitions = {
  standard: "standard",
  xBackground: "x-background",
  xIcon: "x-icon",
} as const;

export type ImageType =
  (typeof imageTypeDefinitions)[keyof typeof imageTypeDefinitions];
