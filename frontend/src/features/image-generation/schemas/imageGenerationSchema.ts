import { z } from "zod";
import { imageTypeDefinitions } from "@/types/image-type";

const printableText = /^\P{Cc}*$/u;
const whitespaceCharacter = /^\p{White_Space}$/u;
const formatCharacter = /^\p{Cf}$/u;
const maximumCodeUnitsPerGrapheme = 1024;
const graphemeSegmenter = new Intl.Segmenter(undefined, {
  granularity: "grapheme",
});
const imageTypeValues = [
  imageTypeDefinitions.standard,
  imageTypeDefinitions.xBackground,
  imageTypeDefinitions.xIcon,
] as const;

const exceedsGraphemeLimit = (value: string, maximumGraphemes: number) => {
  if (value.length > maximumGraphemes * maximumCodeUnitsPerGrapheme) {
    return true;
  }

  let count = 0;
  for (const _ of graphemeSegmenter.segment(value)) {
    if (count === maximumGraphemes) {
      return true;
    }
    count += 1;
  }

  return false;
};

const containsVisibleCharacter = (value: string) => {
  for (const { segment } of graphemeSegmenter.segment(value)) {
    for (const character of segment) {
      if (
        !whitespaceCharacter.test(character) &&
        !formatCharacter.test(character)
      ) {
        return true;
      }
    }
  }

  return false;
};

const addGraphemeLimitIssue = (
  value: string,
  context: z.RefinementCtx,
  maximumGraphemes: number,
  message: string,
) => {
  if (exceedsGraphemeLimit(value, maximumGraphemes)) {
    context.addIssue({ code: "custom", message });
  }
};

export const imageGenerationSchema = z
  .object({
    text: z
      .string()
      .superRefine((value, context) => {
        if (value.trim() !== "" && !printableText.test(value)) {
          context.addIssue({
            code: "custom",
            message: "text.controlCharacter",
          });
        }
      })
      .trim()
      .min(1, { message: "text.required" })
      .superRefine((value, context) => {
        addGraphemeLimitIssue(value, context, 64, "text.maxLength");
      }),
    foregroundCharacter: z
      .string()
      .min(1, { message: "foregroundCharacter.required" })
      .superRefine((value, context) => {
        addGraphemeLimitIssue(
          value,
          context,
          128,
          "foregroundCharacter.maxLength",
        );
      })
      .regex(printableText, {
        message: "foregroundCharacter.controlCharacter",
      }),
    foregroundColor: z.string(),
    backgroundCharacter: z
      .string()
      .min(1, { message: "backgroundCharacter.required" })
      .superRefine((value, context) => {
        addGraphemeLimitIssue(
          value,
          context,
          128,
          "backgroundCharacter.maxLength",
        );
      })
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
      containsVisibleCharacter(foregroundCharacter) ||
      containsVisibleCharacter(backgroundCharacter)
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
