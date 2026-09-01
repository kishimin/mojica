import { zodResolver } from "@hookform/resolvers/zod";
import { useForm } from "react-hook-form";
import {
  imageGenerationSchema,
  type ImageGenerationFormValues,
} from "../schemas/imageGenerationSchema";
import { imageTypeDefinitions } from "@/types/image-type";

const defaultValues: Pick<ImageGenerationFormValues, "type"> = {
  type: imageTypeDefinitions.standard,
};

export const useImageGenerationForm = () =>
  useForm<ImageGenerationFormValues>({
    defaultValues,
    resolver: zodResolver(imageGenerationSchema),
  });
