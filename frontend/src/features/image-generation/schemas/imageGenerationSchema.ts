import { z } from "zod";
import { imageTypeValues } from "@/types/image-type";

const printableText = /^\P{Cc}*$/u;

export const imageGenerationSchema = z
  .object({
    text: z
      .string()
      .regex(printableText, { message: "text.controlCharacter" })
      .trim()
      .min(1, { message: "text.required" })
      .max(64, { message: "text.maxLength" }),
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
    type: z.enum(imageTypeValues, {
      error: "imageType.invalid",
    }),
  })
  .superRefine(({ foregroundCharacter, backgroundCharacter }, context) => {
    if (
      foregroundCharacter.trim() !== "" ||
      backgroundCharacter.trim() !== ""
    ) {
      return;
    }

    context.addIssue({
      code: "custom",
      message: "characterCombination.required",
      path: ["foregroundCharacter"],
    });
    context.addIssue({
      code: "custom",
      message: "characterCombination.required",
      path: ["backgroundCharacter"],
    });
  });

export type ImageGenerationFormValues = z.infer<typeof imageGenerationSchema>;
