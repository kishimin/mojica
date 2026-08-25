# Logo

- Layer: Shared UI
- Location: `components/Logo/Logo.tsx`
- Implementation: `src/assets/logo.svg` + the “mojica” wordmark text
- Responsibility: Shows the logo image next to the wordmark to identify the brand

Use the 1254×1254 SVG from the [kishimin/mojica reference asset](https://github.com/kishimin/mojica/blob/8fc1ef9995d52ec02a6fee242eb7498e9a7c1b49/frontend/src/assets/logo.svg), imported from `src/assets/logo.svg`. Because the image represents the same brand name as the wordmark text, assign `alt=""` to hide it from assistive technology. The visible “mojica” text supplies the accessible name (ui.md §15).

## Storybook

| Main story state | Verification |
| ---------------- | ------------ |
| Default | The logo image and visible “mojica” text appear next to one another, and the text supplies the accessible name |

## Tests

- Size: Small
- Verifies: The logo image and visible “mojica” text are provided, and the image's empty alternative text prevents duplicate announcement
