# ColorPickerField

- Layer: Shared UI
- Location: `components/ColorPickerField/ColorPickerField.tsx`
- Implementation: Native `input[type=color]` + shadcn/ui `Input` (HEX text)
- Responsibility: A color-picker field. The frontend stores its value as a HEX string.

## Props

```typescript
// components/ColorPickerField
type ColorPickerFieldProps = Omit<
  React.ComponentPropsWithoutRef<"input">,
  "type" | "value" | "onChange"
> & {
  label: string;
  value: string; // HEX format (for example, "#FFD400")
  onChange: (hex: string) => void;
  errorMessage?: string;
};
```

## Storybook

| Main story state                         | Verification |
| ---------------------------------------- | ------------ |
| Default / Filled (shows HEX) / Error / Disabled | Use `play` to verify that editing either the HEX text input or the color picker displays the same HEX value in both controls |

## Tests

- Size: Small
- Verifies with `userEvent`: the initial HEX value and error copy are displayed; editing either the HEX text input or the color picker synchronizes both displayed values; and Disabled prevents value changes
