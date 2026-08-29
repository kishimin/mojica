# AppHeader

- Layer: Application shell (`app/components/`)
- Location: `app/components/AppHeader/AppHeader.tsx`
- Implementation: Composition of [Logo](./Logo.md) and [LanguageSwitcher](./LanguageSwitcher.md)
- Responsibility: Positions `Logo` and `LanguageSwitcher` and bridges locale state to the i18n hooks

Components under `components/`, including [Logo](./Logo.md) and [LanguageSwitcher](./LanguageSwitcher.md), have no hook dependencies at all, including i18n hooks. As part of the application shell, however, `AppHeader` depends on i18n hooks such as `useTranslations` and `useLocale`.

## Storybook

| Main story state          | Verification |
| ------------------------- | ------------ |
| Default (ja) / Default (en) | Inject the i18n provider with `decorators` and verify the copy for each locale |

## Tests

- Size: Small
- Verifies: Displayed copy changes when the i18n locale changes
