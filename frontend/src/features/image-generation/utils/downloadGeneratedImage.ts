import type { AxiosResponse } from "axios";

const fallbackFilename = "generated-image.png";

const getFilename = (contentDisposition: string | undefined) => {
  const match = contentDisposition?.match(/filename="?([^";]+)"?/i);

  return match?.[1] ?? fallbackFilename;
};

/** Creates a browser download from the generated PNG response. */
export const downloadGeneratedImage = (response: AxiosResponse<Blob>) => {
  const contentDisposition =
    "get" in response.headers && typeof response.headers.get === "function"
      ? response.headers.get("content-disposition")
      : undefined;
  const filename = getFilename(
    typeof contentDisposition === "string" ? contentDisposition : undefined,
  );
  const objectUrl = URL.createObjectURL(response.data);
  const anchor = document.createElement("a");

  anchor.href = objectUrl;
  anchor.download = filename;
  anchor.click();
  URL.revokeObjectURL(objectUrl);
};
