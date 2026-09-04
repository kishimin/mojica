import { imageTypeDefinitions } from "@/types/image-type";

export const imageTypeOptions = Object.values(imageTypeDefinitions).map(
  (value) => ({ value }),
);
