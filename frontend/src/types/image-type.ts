/** Image type values exposed by the image-generation UI. */
export const imageTypeValues = ["standard", "x-background", "x-icon"] as const;

export type ImageType = (typeof imageTypeValues)[number];
