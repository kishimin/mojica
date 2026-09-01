import { zodResolver } from "@hookform/resolvers/zod";
import { useForm, type DefaultValues } from "react-hook-form";
import {
  imageGenerationSchema,
  type ImageGenerationFormValues,
} from "../schemas/imageGenerationSchema";
import { imageTypeDefinitions } from "@/types/image-type";

const defaultValues = {
  text: "KA",
  foregroundCharacter: "",
  foregroundColor: "#FFD400",
  backgroundCharacter: "☀",
  backgroundColor: "#FF69B4",
  type: imageTypeDefinitions.standard,
} satisfies DefaultValues<ImageGenerationFormValues>;

export const useImageGenerationForm = () =>
  useForm<ImageGenerationFormValues>({
    defaultValues,
    resolver: zodResolver(imageGenerationSchema),
  });
