# mojica MVP Component Design

This document decomposes [ui.md](./ui.md) into React components. The implementation uses React, TypeScript, Vite, TanStack Query, Axios, Zod, React Hook Form, Orval, TanStack Router, Tailwind, shadcn/ui, Storybook, Vitest, MSW, and Playwright.

# 1. Component categories and placement

| Layer | Responsibility | Allowed dependencies | Location |
| ----- | -------------- | -------------------- | -------- |
| Shared UI | Display and accessibility; extends native element props/events | None: no direct global state, routing, or fetch imports | `components/` |
| Application shell | Bridges cross-cutting state such as locale into shared UI | Application state such as React Context | `app/components/` |
| Feature UI | mojica API submission, client validation, and downloads | Form state and `POST /images` | `features/image-generation/` |

```text
src/
├── app/
│   ├── components/{AppHeader,AppFooter,Layout}/
│   ├── providers/AppProviders.tsx
│   └── views/App.tsx
├── components/
│   ├── ui/                         # shadcn/ui generated primitives
│   └── {Logo,LanguageSwitcher,TextField,ColorPickerField,FieldError,AlertBanner}/
├── features/
│   ├── image-generation/
│   │   ├── components/{ImageGenerationForm,ImageTypeSelect,GenerateButton}/
│   │   ├── hooks/{useImageGenerationForm,useRetryAfterCountdown}.ts
│   │   ├── schemas/imageGenerationSchema.ts
│   │   └── views/ImageGenerationScreen.tsx
│   ├── not-found/views/NotFoundView.tsx
│   └── error/views/ErrorFallback.tsx
├── gen/api/                        # Orval generated; do not edit
├── lib/{queryClient,router}.ts
├── routes/{__root,index}.tsx
└── routeTree.gen.ts                # generated; do not edit
```

Each component's contract is defined under [components](./components/). Colocate its implementation, story, and Small test. Keep generated shadcn primitives under `components/ui/` and follow [ShadcnUiWrappers.md](./components/ShadcnUiWrappers.md).

`AppProviders` puts `ErrorBoundary` outermost, then `QueryClientProvider`, then `I18nProvider`. `I18nProvider` persists `"ja"` or `"en"` under local-storage key `"locale"`; `ErrorFallback` reads the same key without depending on that provider. `App` composes the providers and router. `Layout` owns the header, outlet, and footer. The generated route tree is never edited manually.

# 2. State model

Keep input state and validation in React Hook Form with the Zod schema. Keep server mutation state in the generated TanStack Query hook instead of copying it into a UI type. `GenerateButton` alone receives an exclusive discriminated union for presentation: idle, submitting, retryable, or cooldown. `ImageGenerationForm` decides precedence once. Keep locale in `I18nProvider`, not in individual shared components.

# 3. Asynchronous boundary

There is no initial query and therefore no Suspense boundary. `POST /images` is a mutation whose pending, success, and error states remain local to the form. API failures are expected data and do not reach ErrorBoundary. One root ErrorBoundary catches unexpected rendering exceptions and displays `ErrorFallback` outside every provider.

# 4. Effects on i18n, accessibility, and responsive behavior

- **i18n**: Render labels, buttons, options, and client validation through translations. The server localizes API `message` according to `Accept-Language`; use only `code` and `errors[].field` for UI decisions and display `message` unchanged (ui.md §13).
- **Accessibility**: Associate fields and `FieldError` with `aria-describedby`; use empty `alt` for the logo image duplicated by its wordmark; mark decorative icons `aria-hidden="true"`; use `role="alert"` for `AlertBanner`; and communicate pending state with both `aria-busy` and visible copy.
- **Responsive behavior**: Use one form column. `ImageGenerationScreen` owns centering and maximum width. Shared controls normally use `w-full` and must not introduce horizontal scrolling.

# 5. Effects on the existing API contract

Do not change API values to match UI labels. Derive request types and image-type values from Orval output, while translation dictionaries own labels. Send colors as HEX, locale as `Accept-Language`, and the validated request body to `POST /images`. Treat `422 errors[].field`, language-independent error `code`, localized `message`, `Retry-After`, `Content-Disposition`, and the PNG body according to [api.md](../api/api.md).

# 6. Tests and residual risks

## Test policy

Use Small tests for pure schemas, hooks, components, provider wiring, and the MSW-backed same-process form integration. Do not test generated Orval code or Radix/shadcn internals twice. Use Medium/Large only according to actual external dependencies, not the test framework name.

## Storybook and accessibility

- Every documented story must pass `storybook build` and `vitest --project=storybook` through `@storybook/addon-vitest`.
- Every story must pass `@storybook/addon-a11y` axe checks.
- Stories represent states, not separate Mobile/Tablet variants. Verify responsive stories at the 390px, 768px, and 1440px viewports from [design-tokens.md](./design-tokens.md) §6.
- Verify keyboard-only input, color choice, image type, language switching, and submission; screen-reader error associations; and complete copy updates after locale changes.

## E2E (`frontend/e2e/`)

- Golden path from input through generation to actual browser download detection.
- VRT baselines for Default, Filled, Submitting, API Error (Retryable button + banner), and 404, fixed to Desktop Chrome with dynamic time and randomness stabilized.
- Direct navigation to an unknown URL and the Back to Home path.
- Mobile/tablet responsive layouts matching the Figma frames.

## CI schedule

| Size | Events |
| ---- | ------ |
| Small | push, pull_request, nightly schedule, workflow_dispatch |
| Medium | pull_request, nightly schedule, workflow_dispatch |
| Large | nightly schedule, workflow_dispatch |

## Risks

- Native `input[type=color]` UI differs by browser and OS; this is accepted.
- Browser compatibility for Blob conversion and parsing `filename` from `Content-Disposition` requires E2E download coverage.
