# `feat/mojica-ui-foundation` Work Plan

## Branch

- Branch: `feat/mojica-ui-foundation`
- Start point: `main` at `5fd5558d98eb3b374ced91ad8c8dc28af69582ce`
- Integration target: `feat/mojica-mvp-ui`
- Merge order: first child branch

## Sources

- [UI branch implementation plan](../implementation-plan.md)
- [UI specification](../ui.md)
- [Component design](../component-design.md)
- [Design tokens](../design-tokens.md)
- [Frontend architecture](../frontend-architecture.md)

## Prerequisites and dependencies

- The branch starts from `main` and has no preceding UI child-branch dependency.
- `feat/mojica-mvp-ui` must exist as the parent integration branch before this branch is integrated.
- Generated API and Zod artifacts must remain derived from their declared generation sources; generated files are not edited manually.

## Owned scope from the implementation plan

- Material Design 3 CSS custom properties.
- Shared CSS and responsive foundation.
- Axios, Orval, and TanStack Query wiring.
- Shared Zod schema placement.
- Minimal i18n and provider setup.

## Work items

The decomposition below is inferred from the owned scope and linked specifications. Completion must be established separately with repository evidence.

### Design tokens and shared styles

- [ ] Define the approved semantic color and radius values as CSS custom properties.
- [ ] Map Tailwind and shadcn utilities to the semantic tokens without duplicating component-local color literals.
- [ ] Establish the shared light-theme, typography, spacing, and responsive foundations required by the 390px, 768px, and 1440px design targets.
- [ ] Keep dark-theme values out of scope until a dark-theme design is approved.

### API and server-state foundation

- [ ] Configure Axios through the shared request boundary used by generated API clients.
- [ ] Configure Orval inputs and outputs so generated API and Zod files have declared sources and stable placement.
- [ ] Configure the shared TanStack Query client and expose it through the application provider boundary.
- [ ] Preserve API option values and generated request types independently from localized UI labels.

### Shared schema placement

- [ ] Place shared generated Zod schemas at the documented generated-code boundary.
- [ ] Keep hand-written feature validation out of the foundation branch and reserve it for `feat/mojica-ui-image-generation`.

### i18n and providers

- [ ] Define the supported locale contract for Japanese and English.
- [ ] Provide locale state and updates through `I18nProvider` and reject hook consumers outside that provider.
- [ ] Persist only supported locale values under the `locale` local-storage key and resolve the initial locale according to the documented fallback behavior owned by the provider.
- [ ] Compose `QueryClientProvider` and `I18nProvider` through a shared `AppProviders` boundary using the same query client used by consumers.
- [ ] Keep the root `ErrorBoundary` and its fallback implementation out of this branch because error screens belong to `feat/mojica-ui-error-pages`.

### Tests and integration readiness

- [ ] Cover the observable contracts of the query-client, i18n, and provider wiring with appropriately sized tests.
- [ ] Confirm generated artifacts are current without adding tests that duplicate generated-library behavior.
- [ ] Confirm the branch contains no shared UI components, image-generation screen behavior, error-page routing, or E2E scenarios owned by later branches.
- [ ] Record scope, verification evidence, and unfinished work in the child PR before integration.

## Non-goals

- Shared UI components: `Logo`, `FieldError`, `TextField`, `ColorPickerField`, `AlertBanner`, `LanguageSwitcher`, `ImageTypeSelect`, `GenerateButton`, `AppHeader`, `AppFooter`, and `Layout`.
- Image-generation form behavior, client validation, `POST /images`, API error presentation, retry countdown, and PNG download.
- The 404 screen, root `ErrorBoundary`, `ErrorFallback`, route wiring, and reload behavior.
- Playwright flows, browser download checks, VRT baselines, and other E2E work.
- Image preview, custom image sizes, history, login, user storage, social sharing, and server-side generated-image storage.

## Completion criteria

- [ ] Design documents and implementation agree within the foundation scope.
- [ ] Every available formatting check succeeds. No formatting command is currently declared in `frontend/package.json`; completion verification must record this as unavailable unless repository configuration changes.
- [ ] `bun run typecheck` succeeds from `frontend/`.
- [ ] `bun run lint` succeeds from `frontend/`.
- [ ] `bun run test` succeeds from `frontend/`.
- [ ] `bun run test:small` succeeds from `frontend/` and its overall Statements, Branches, Functions, and Lines metrics are each at least 80%.
- [ ] `bun run build` succeeds from `frontend/`.
- [ ] The child PR records scope, verification, and unfinished work.
- [ ] The completed branch is ready to merge into `feat/mojica-mvp-ui` without absorbing responsibilities from later child branches.

## Unresolved items and blockers

- Completion evidence has not yet been audited against this checklist.
- The most recently observed Small-test coverage was below the required 80% for some overall metrics; the completion audit must obtain current results rather than relying on that earlier observation.
- No repository-defined frontend formatting command is currently available.
