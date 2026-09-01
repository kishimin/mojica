import { zodResolver } from "@hookform/resolvers/zod";
import { useForm } from "react-hook-form";
import { imageTypeDefinitions } from "@/types/image-type";
import {
  imageGenerationSchema,
  type ImageGenerationFormValues,
} from "../schemas/imageGenerationSchema";

const defaultValues: ImageGenerationFormValues = {
  type: imageTypeDefinitions.standard,
};

export const useImageGenerationForm = () =>
  useForm<ImageGenerationFormValues>({
    defaultValues,
    resolver: zodResolver(imageGenerationSchema),
  });
