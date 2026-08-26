# FieldError

- Layer: Shared UI
- Location: `components/FieldError/FieldError.tsx`
- Implementation: Tailwind only (no Radix dependency)
- Responsibility: Displays one error message. Renders nothing when empty.

## Storybook

| Main story state                                  | Verification |
| ------------------------------------------------- | ------------ |
| Default (with message) / Empty (renders nothing)  | Confirm that an empty string is not rendered into the DOM |

## Tests

- Size: Small
- Verifies: Prop-driven display and state transitions
