# ImageGenerationScreen

- Layer: Feature UI
- Location: `features/image-generation/views/ImageGenerationScreen.tsx`
- Implementation: Composition of [ImageGenerationForm](./ImageGenerationForm.md)
- Responsibility: The page body. Renders `ImageGenerationForm`; [Layout](./Layout.md) provides `AppHeader` and `AppFooter`.

The form uses a single-column layout by default. `ImageGenerationScreen`, as the page container, owns maximum width and horizontal centering (ui.md §14).

## Storybook

| Main story state | Verification |
| ---------------- | ------------ |
| Default / Submitting / Success (automatic download) / Error (400/429/500/502/504) | Mock `POST /images` with MSW and reproduce success, each error, and timeout with fixed data |

MSW's `http.post` mocks `POST /images`; stories do not connect to the real API.

## Tests

- Size: Small
- Verifies: Only that `ImageGenerationForm` is rendered; deeper coverage belongs to [ImageGenerationForm](./ImageGenerationForm.md)
