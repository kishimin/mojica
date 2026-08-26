# App

- Layer: App View (`app/views/`)
- Location: `app/views/App.tsx`
- Implementation: Composition of `AppProviders` and `RouterProvider`
- Responsibility: The root view. It is rendered by `main.tsx` and wraps `RouterProvider` in `AppProviders`. The router is the `router` from `lib/router.ts`, created with `createRouter({ routeTree })` from the generated `routes/routeTree.gen.ts`.

`AppProviders` (`app/providers/AppProviders.tsx`) handles wiring. It places `ErrorBoundary` at the outermost application root, followed by `QueryClientProvider` and `I18nProvider` inside it, in that order.

## Tests

| Size                          | Verification |
| ----------------------------- | ------------ |
| Small (`App.small.test.tsx`)  | Verifies the render path through `AppProviders` and `RouterProvider`, plus input → submit → success/error using `App` as the entry point, all in one file. MSW intercepts `POST /images` in the same process before it reaches the real network, and the scope includes the wiring of `QueryClientProvider`, `I18nProvider`, `RouterProvider`, and `Layout`. Integrating multiple modules and using MSW do not by themselves make the test Medium. Route-transition coverage belongs to the `routes/__root.tsx` test. Scenario overlap with the [ImageGenerationForm](./ImageGenerationForm.md) tests is intentional because their scopes differ: isolated behavior versus integration including provider wiring. |

The Small test for `app/providers/AppProviders.tsx` itself verifies that `ErrorBoundary` catches an exception from a child and displays [ErrorFallback](./ErrorFallback.md) (`AppProviders.small.test.tsx`).
