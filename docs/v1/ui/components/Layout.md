# Layout

- Layer: Application shell (`app/components/`)
- Location: `app/components/Layout/Layout.tsx`
- Implementation: [AppHeader](./AppHeader.md) + `<Outlet />` + [AppFooter](./AppFooter.md)
- Responsibility: Wraps every router route in the shared layout

It is assigned as the `component` of `createRootRoute` in `routes/__root.tsx`. TanStack Router's `<Outlet />` is the placeholder for the matched child route. It receives `routes/index.tsx` ([ImageGenerationScreen](./ImageGenerationScreen.md)) under normal navigation and `notFoundComponent` ([NotFoundView](./NotFoundView.md)) for an undefined path. Consequently, neither `ImageGenerationScreen` nor `NotFoundView` owns the header or footer.

## Storybook

| Main story state | Verification |
| ---------------- | ------------ |
| Default (inject dummy content into `<Outlet />`) | Confirm that `AppHeader` and `AppFooter` are always visible and the child is rendered in the center |

## Tests

- Size: Small
- Verifies: `Layout` always displays `<Outlet />`, `AppHeader`, and `AppFooter`
