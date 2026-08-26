# mojica Design Tokens (Material Design 3)

These tokens translate the approved Figma values into Material Design 3-aligned CSS custom properties. Token values are authoritative; components must consume tokens rather than repeat literals.

# 1. Colors

| Token | HEX | OKLCH | Material 3 role | Figma usage |
| ----- | --- | ----- | --------------- | ----------- |
| `background` | `#F2F9FC` | `oklch(0.978 0.008 225.1)` | background | Page background |
| `surface` | `#FFFFFF` | `oklch(1.000 0.000 0)` | card | Header, footer, form card, and fields |
| `foreground` | `#1F1C1A` | `oklch(0.229 0.006 56.1)` | foreground | Primary headings, labels, and body copy |
| `muted-foreground` | `#6B635C` | `oklch(0.505 0.015 63.7)` | muted-foreground | Intro description and language chevron |
| `helper-foreground` | `#496978` | `oklch(0.502 0.044 228.7)` | custom | Length hints and footer copyright |
| `border` | `#DBD4C9` | `oklch(0.872 0.017 79.3)` | border | Language switcher and empty fields |
| `border-accent` | `#BEDEEB` | `oklch(0.881 0.038 224.3)` | outline-variant | Fields, picker, and Select borders |
| `primary` | `#7CC7E8` | `oklch(0.793 0.088 227.9)` | primary | Normal generate-button background |
| `primary-foreground` | `#193A48` | `oklch(0.330 0.046 228.2)` | primary-foreground | Normal generate-button text |
| `inverse` | `#211F1C` | `oklch(0.240 0.006 78.2)` | custom | Retryable and Back to Home buttons |
| `inverse-foreground` | `#FFFFFF` | `oklch(1.000 0.000 0)` | custom | Text on inverse |
| `destructive` | `#C72929` | `oklch(0.541 0.194 26.7)` | destructive | Validation borders and error copy |
| `destructive-background` | `#FFF2F2` | `oklch(0.971 0.014 17.4)` | custom | AlertBanner background |
| `destructive-border` | `#EB8C8C` | `oklch(0.740 0.116 20.2)` | custom | AlertBanner border |

# 2. Typography

| Token | Size | Weight | Usage |
| ----- | ---- | ------ | ----- |
| `text-xs` | 12px | 400 | Hints, footer, automatic-download note |
| `text-xs-medium` | 13px | 600 / 400 | Language name / alert description |
| `text-sm` | 14px | 400 / 600 | Intro description / labels and alert heading |
| `text-base` | 15px | 400 / 600 | Color and Select values / Back to Home |
| `text-md` | 16px | 600 | Generate button |
| `text-lg` | 18px | 600 | Select chevron |
| `text-2xl` | 22px | 700 | “mojica” wordmark |
| `text-3xl` | 24px | 700 | Mobile intro heading |
| `text-4xl` | 26px | 700 | Not-found heading |
| `text-5xl` | 30px | 700 | Desktop/tablet intro heading |
| `text-7xl` | 72px | 700 | “404” |

# 3. Border radius

| Token | Value | Usage |
| ----- | ----- | ----- |
| `radius-sm` | 8px | Color swatch |
| `radius-md` | 10px | Fields, Select, switcher, picker, and alert |
| `radius-lg` | 12px | Generate and Back to Home buttons |
| `radius-xl` | 18px | Form card |

# 4. Spacing

Use the Material 3 4px grid. Principal measurements:

| Element | Padding | Gap | Height |
| ------- | ------- | --- | ------ |
| Header | `0 56px` desktop / `0 32px` tablet / `0 20px` mobile | — | 88px desktop/tablet; 72px mobile |
| Logo image + wordmark | — | 12px | — |
| Main content | `48px 0 0` desktop/tablet; `32px 0 0` mobile | 28px desktop/tablet; 24px mobile | — |
| Intro | — | 12px | — |
| Form card | 32px desktop; `24px 20px` mobile | 24px | — |
| Field group | — | 8px | — |
| Text field | `0 16px` | 8px | 48px |
| Color picker | `0 16px 0 12px` | 12px | 56px |
| Swatch | — | — | 36×36 |
| Select | `0 16px` | — | 48px |
| Generate button | — | — | 56px |
| API alert | 16px | 8px | — |
| Footer | — | — | 80px |

# 5. Effects

| Token | Value | Usage |
| ----- | ----- | ----- |
| `shadow-card` | `0px 8px 24px 0px rgba(0, 0, 0, 0.08)` | Form-card shadow |

# 6. Breakpoints

| Name | Width | Figma frame |
| ---- | ----- | ----------- |
| Mobile | 390px | `mojica / Mobile / JA` |
| Tablet | 768px | `mojica / Tablet / JA` |
| Desktop | 1440px | `mojica / Default`, `Desktop / EN` |

# 7. CSS custom-property policy

Define semantic color and radius variables at `:root`, map Tailwind/shadcn utilities to them, and keep light-theme values as the MVP source of truth. Do not introduce a dark palette until a dark-theme design is approved. Typography and spacing use project Tailwind theme extensions only when the standard scale cannot express the approved values.

# 8. Component mapping

| Component | Colors | Radius | Typography |
| --------- | ------ | ------ | ---------- |
| [Layout](./components/Layout.md) | `background` | — | — |
| Header, footer, and form card | `surface` | `radius-xl` for card | — |
| [Logo](./components/Logo.md) | — | — | `text-2xl` wordmark |
| [TextField](./components/TextField.md) | `border`, `border-accent` | `radius-md` | `text-sm`, `text-xs` |
| [ColorPickerField](./components/ColorPickerField.md) | `border-accent` | `radius-md`, `radius-sm` | `text-base` |
| [LanguageSwitcher](./components/LanguageSwitcher.md) | `border` | `radius-md` | `text-xs-medium` |
| [FieldError](./components/FieldError.md) | `destructive` | — | `text-xs` |
| [AlertBanner](./components/AlertBanner.md) | destructive background/border/text | `radius-md` | `text-sm`, `text-xs-medium` |
| [GenerateButton](./components/GenerateButton.md) normal | primary pair | `radius-lg` | `text-md` |
| GenerateButton Retryable and Back to Home | inverse pair | `radius-lg` | `text-md`, `text-base` |
| [ImageTypeSelect](./components/ImageTypeSelect.md) | `border-accent` | `radius-md` | `text-base` |
| [NotFoundView](./components/NotFoundView.md) | foreground and muted foreground | — | `text-7xl`, `text-4xl` |

# 9. Remaining items

Confirm final focus, hover, disabled, and pressed-state values against the corresponding Figma component states during implementation. Record any approved token change here before applying component-local literals.

## Reference

- Material Design 3 color roles and shape guidance
- Figma frames listed in §6
