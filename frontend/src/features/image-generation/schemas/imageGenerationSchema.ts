import { z } from "zod";

export const imageGenerationSchema = z.object({
  text: z.string(),
  foregroundCharacter: z.string(),
  foregroundColor: z.string(),
  backgroundCharacter: z.string(),
  backgroundColor: z.string(),
  type: z.enum(["standard", "x-background", "x-icon"]),
});

export type ImageGenerationFormValues = z.infer<
  typeof imageGenerationSchema
>;
