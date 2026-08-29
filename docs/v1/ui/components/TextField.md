# TextField

- Layer: Shared UI
- Location: `components/TextField/TextField.tsx`
- Implementation: Composes shadcn/ui `Input` + `Label` + [FieldError](./FieldError.md)
- Responsibility: An input field combining a label, input, and `FieldError`. Used for the text to render, the character used to render it, and the background fill character.

## Props

```typescript
// An application-owned component under components/TextField that composes ui/Input and ui/Label
type TextFieldProps = React.ComponentPropsWithoutRef<"input"> & {
  label: string;
  errorMessage?: string;
};
```

Do not redefine native props such as `onClick`, `disabled`, `type`, or `aria-*` with a custom signature.

## Storybook

| Main story state | Verification |
| ---------------- | ------------ |
| Default / Filled / Error (with validation message) / Disabled | It can be queried as a textbox with the label using `getByRole("textbox", { name: label })`, and the error message is associated as an accessible description |

## Tests

- Size: Small
- Verifies: Prop-driven display, `userEvent` interaction, and state transitions
