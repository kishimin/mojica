# LanguageSwitcher

- Layer: Shared UI
- Location: `components/LanguageSwitcher/LanguageSwitcher.tsx`
- Implementation: shadcn/ui `DropdownMenu` (Radix UI Dropdown Menu) + Lucide `ChevronDown`
- Responsibility: A controlled dropdown showing the selected language name and an expand icon

This controlled component receives `locale`, `options`, and `onChange`. [AppHeader](./AppHeader.md) connects it to the actual i18n hooks.

## Storybook

| Main story state | Verification |
| ---------------- | ------------ |
| Default (closed) / Open (ja selected) / Open (en selected) / Keyboard Focus | Keyboard interaction (arrow keys, Enter, Esc) and the ARIA representation of `role="menu"` |

## Tests

- Size: Small
- Verifies: Prop-driven display, `userEvent` interaction, and state transitions
