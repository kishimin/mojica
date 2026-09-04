# ErrorFallback

- Layer: Feature UI (`features/error/views/`)
- Location: `features/error/views/ErrorFallback.tsx`
- Implementation: Tailwind only
- Responsibility: The `ErrorBoundary` fallback UI. When an unexpected exception occurs during rendering, it replaces the application content without rendering [AppHeader](./AppHeader.md) or [AppFooter](./AppFooter.md) (ui.md §20).

Although it has no API calls or form state, it is a screen for the error feature and is treated like [NotFoundView](./NotFoundView.md). Following the frontend-folder-structure placement workflow, it belongs in `features/error/views/` rather than `app/components/`.

## Difference from the 404 screen

`NotFoundView` replaces only the contents of [Layout](./Layout.md)'s `<Outlet />`, so `AppHeader` and `AppFooter` remain visible. `ErrorFallback` is rendered directly by the root `ErrorBoundary` and therefore does not render the header or footer. `NotFoundView` participates in routing as the `notFoundComponent` of `routes/__root.tsx`; `ErrorFallback` is rendered independently of routing.

Place `ErrorBoundary` at the outermost edge of `AppProviders`, outside `QueryClientProvider` and `I18nProvider`. This ensures that `ErrorFallback` remains available even if `I18nProvider` itself causes an exception. Accordingly, `ErrorFallback` does not use the `I18nProvider` React context.

## i18n

Instead of a translation function such as `useTranslations`, `ErrorFallback` selects its copy from a minimal dictionary embedded in the component. It operates outside the `I18nProvider` context tree and can therefore preserve locale-appropriate output even if `I18nProvider` crashes. Derive the supported-locale type from the dictionary keys so adding a language does not add branches to locale resolution.

```typescript
// features/error/views/ErrorFallback.tsx (illustrative)
const messages = {
  ja: {
    heading: "エラーが発生しました",
    description:
      "予期しない問題が発生しました。しばらくしてからページを再読み込みしてください。",
    button: "ページを再読み込み",
  },
  en: {
    heading: "An error occurred",
    description:
      "Something unexpected happened. Please reload the page and try again.",
    button: "Reload page",
  },
} as const;

type SupportedLocale = keyof typeof messages;

const defaultLocale: SupportedLocale = "ja";

const isSupportedLocale = (value: string): value is SupportedLocale =>
  Object.hasOwn(messages, value);

const readStoredLocale = (): string | null => {
  try {
    return localStorage.getItem("locale");
  } catch {
    return null;
  }
};

const resolveLocale = (): SupportedLocale => {
  const candidates = [readStoredLocale(), ...navigator.languages];

  for (const candidate of candidates) {
    const normalized = candidate?.toLowerCase();
    if (normalized && isSupportedLocale(normalized)) return normalized;

    const language = normalized?.split("-")[0];
    if (language && isSupportedLocale(language)) return language;
  }

  return defaultLocale;
};
```

Read the `"locale"` local-storage key directly—the same key `I18nProvider` (`providers/I18nProvider.tsx`) uses for persistence (see component-design.md). If the stored value is absent or unsupported, or local storage cannot be read, inspect `navigator.languages` in order. Match the full language tag first, followed by its leading language subtag, such as `en` from `en-US`. If no browser language is supported, fall back to the default locale, `ja`. Dictionary keys are lowercase language tags. Add any new language to the dictionary and keep it aligned with the locales supported by `I18nProvider`.

## Screen specification (ui.md §20)

- Heading: `エラーが発生しました` (en: “An error occurred”)
- Description: `予期しない問題が発生しました。しばらくしてからページを再読み込みしてください。` (en: “Something unexpected happened. Please reload the page and try again.”)
- Button: `ページを再読み込み` (en: “Reload page”)

The button performs a normal browser page reload equivalent to `window.location.reload()`, not client-side navigation. The root `ErrorBoundary` may be handling a corrupted React tree that includes providers, so application navigation alone cannot guarantee recovery from a clean initial state. Its click handler must not call the mojica API directly. Any network activity after the reload follows the normal initial-render flow at that time.

## Storybook

| Main story state | Verification |
| ---------------- | ------------ |
| Default (default locale) / Supported Locale (representative non-default locale) / Unsupported Locale | Displayed heading, description, and button; precedence among stored value, browser language, and default locale |

## Tests

- Size: Small
- Verifies: `ErrorFallback` in isolation; copy for every locale in the dictionary; preference for a stored value; fallback to browser language or the default locale when the stored value is absent, unsupported, or unreadable; and, using `userEvent`, that the reload button invokes page reload. `AppProviders.small.test.tsx` verifies that `ErrorBoundary` actually catches a child exception and renders `ErrorFallback` (see [App](./App.md)).
