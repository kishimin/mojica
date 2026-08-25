# NotFoundView

- Layer: Feature UI (`features/not-found/views/`)
- Location: `features/not-found/views/NotFoundView.tsx`
- Implementation: Tailwind only
- Responsibility: The 404 Not Found screen. It is rendered as `notFoundComponent` by `routes/__root.tsx` and provides a link back to the home page.

Although it differs from [ImageGenerationForm](./ImageGenerationForm.md) and similar components because it has no API calls or form state, it is still a screen representing a feature. Following the frontend-folder-structure placement workflow, it therefore belongs in the independent `features/not-found/views/` directory rather than `app/views/`. The `features/` directory is not limited to features that call the mojica API; it also includes self-contained screens without external dependencies, such as the 404 view.

## Storybook

| Main story state | Verification |
| ---------------- | ------------ |
| Default | Display of the heading, description, and `トップページへ戻る` (Back to home) link |

## Tests

- Size: Small
- Verifies: Display of the heading, description, and `トップページへ戻る` (Back to home) link
