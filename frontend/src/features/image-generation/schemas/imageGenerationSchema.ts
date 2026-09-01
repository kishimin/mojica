import { z } from "zod";

const printableText = /^\P{Cc}*$/u;

export const imageGenerationSchema = z
  .object({
    text: z
      .string()
      .trim()
      .min(1, { message: "text.required" })
      .max(64, { message: "text.maxLength" })
      .regex(printableText, { message: "text.controlCharacter" }),
    foregroundCharacter: z
      .string()
      .min(1, { message: "foregroundCharacter.required" })
      .max(128, { message: "foregroundCharacter.maxLength" })
      .regex(printableText, {
        message: "foregroundCharacter.controlCharacter",
      }),
    foregroundColor: z.string(),
    backgroundCharacter: z
      .string()
      .min(1, { message: "backgroundCharacter.required" })
      .max(128, { message: "backgroundCharacter.maxLength" })
      .regex(printableText, {
        message: "backgroundCharacter.controlCharacter",
      }),
    backgroundColor: z.string(),
    type: z.enum(["standard", "x-background", "x-icon"]),
  })
  .refine(
    ({ foregroundCharacter, backgroundCharacter }) =>
      foregroundCharacter.trim() !== "" || backgroundCharacter.trim() !== "",
    { message: "characterCombination.required", path: ["foregroundCharacter"] },
  );

export type ImageGenerationFormValues = z.infer<typeof imageGenerationSchema>;
