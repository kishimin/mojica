# ImageTypeSelect

- Layer: Feature UI
- Location: `features/image-generation/components/ImageTypeSelect/ImageTypeSelect.tsx`
- Implementation: Wraps shadcn/ui `Select`
- Responsibility: Maps image-type values generated from OpenAPI to localized display labels

## Props

```typescript
// features/image-generation/components/ImageTypeSelect
import type { ImageGenerationRequest } from "@/gen/api";

type ImageType = ImageGenerationRequest["type"];

type ImageTypeSelectProps = {
  value: ImageType;
  onChange: (value: ImageType) => void;
  errorMessage?: string;
};
```

The actual generated type name and import path follow the OpenAPI schema name and Orval output. Do not redefine a string union of API values inside `ImageTypeSelect`; derive it from the generated request type's `type` property.

If Orval generates the image-type enum as a runtime object, build the option values from that output. If it generates only a type, statically check the UI-defined value list against `ImageType` so that values absent from the OpenAPI contract cannot be added.

Display labels such as `標準画像` (Standard image) are UI copy, not API values. They therefore do not belong in generated OpenAPI output; look them up from the i18n translation dictionary using the API value as the key. Option values track the generated API type, while display labels track the translation dictionary (see component-design.md §5 for effects on the existing API contract).

## Storybook

| Main story state | Verification |
| ---------------- | ------------ |
| Default (`standard`) / switch to each option | Use the keyboard to select an image type and verify that the selected item changes (`play`) |

## Tests

- Size: Small
- Verifies: Prop-driven display state
