# `feat/mojica-ui-components` Work Plan

## Branch

- Branch: `feat/mojica-ui-components`
- Start point: `origin/main` at `b3c0335`
- Integration target: `feat/mojica-mvp-ui`
- Merge order: second UI work unit, after foundation

## Sources

- [UI branch implementation plan](../implementation-plan.md)
- [UI specification](../ui.md)
- [Component design](../component-design.md)
- [Design tokens](../design-tokens.md)
- [Shadcn UI wrapper design](../components/ShadcnUiWrappers.md)
- Component contracts: [Logo](../components/Logo.md), [FieldError](../components/FieldError.md), [TextField](../components/TextField.md), [ColorPickerField](../components/ColorPickerField.md), [AlertBanner](../components/AlertBanner.md), [LanguageSwitcher](../components/LanguageSwitcher.md), [ImageTypeSelect](../components/ImageTypeSelect.md), [GenerateButton](../components/GenerateButton.md), [AppHeader](../components/AppHeader.md), [AppFooter](../components/AppFooter.md), [Layout](../components/Layout.md)

## Prerequisites and dependencies

- Foundation work is included in `origin/main` at the selected start point.
- Use the existing CSS semantic tokens, i18n provider, query provider, aliases, and generated API boundaries; do not recreate them locally.
- Generated shadcn primitives belong under `frontend/src/components/ui/` and must remain regenerable by the CLI.
- Image-generation business behavior, routing, and error-page behavior remain owned by later branches.

## Owned scope

The branch implements shared UI and application-shell components required by the MVP:

- Shared UI: `Logo`, `FieldError`, `TextField`, `ColorPickerField`, `AlertBanner`, and `LanguageSwitcher`.
- Feature-facing controls assigned to this branch: `ImageTypeSelect` and `GenerateButton`.
- Application shell: `AppHeader` and `AppFooter`.
- Required shadcn/ui primitives, colocated stories, and behavior-focused Small tests for application-owned components.

## Ordered work items

### 1. Shadcn primitives and shared UI

- [ ] Generate or add the required shadcn primitives under `frontend/src/components/ui/` without editing vendor behavior manually.
- [ ] Implement `Logo` with the SVG asset, visible `mojica` wordmark, and empty image alternative text.
- [ ] Implement `FieldError` so it renders one message and renders nothing for an empty value.
- [x] Implement `TextField` with a label, native input props, and accessible error association. (Implemented in `e3b1714`; behavior specified in `316af34`.)
- [ ] Implement `ColorPickerField` with synchronized native color and HEX text controls, including disabled behavior.
- [ ] Implement `AlertBanner` with `role="alert"`, title, description, and the approved alert composition.
- [ ] Implement `LanguageSwitcher` as a controlled accessible dropdown that reports locale changes without owning i18n state.

### 2. Feature-facing controls

- [ ] Implement `ImageTypeSelect` using the generated API image-type value contract and localized display labels.
- [ ] Implement `GenerateButton` using the exclusive idle, submitting, retryable, and cooldown state union.
- [ ] Ensure submitting/cooldown disabled state, `aria-busy`, copy, loader, and retryable styling match the component contract.

### 3. Application shell

- [ ] Implement `AppHeader` by composing `Logo` and `LanguageSwitcher` through the i18n boundary.
- [ ] Implement `AppFooter` with the documented copyright text.
- `Layout` composition is deferred to a follow-up branch.

### 4. Stories, tests, and accessibility

- [ ] Add the documented Storybook states for each application-owned component.
- [ ] Add Small tests using semantic queries and `userEvent` for documented interaction and state contracts.
- [ ] Verify keyboard operation, labels, descriptions, error associations, empty image alt text, alert role, and pending state semantics.
- [ ] Verify responsive stories at 390px, 768px, and 1440px without adding viewport-specific duplicate components.

## Non-goals

- Image-generation form, client validation, `POST /images`, API error mapping, retry countdown, and PNG download.
- 404 and unexpected-error screens, ErrorBoundary, route wiring, and browser reload behavior.
- `Layout` composition and its route integration.
- Playwright E2E flows, VRT baselines, download verification, and deployed API integration.
- Changes to generated API clients, generated Zod schemas, or server behavior.

## Completion criteria

- Design documents and component implementations agree within this branch's scope.
- Every available formatting check succeeds. No formatter script is currently declared in `frontend/package.json`.
- `bun run typecheck` succeeds from `frontend/`.
- `bun run lint` succeeds from `frontend/`.
- `bun run test` and `bun run test:small` succeed from `frontend/`.
- `bun run test:storybook` and `bun run build-storybook` succeed once stories exist.
- Overall coverage reports Statements, Branches, Functions, and Lines at 80% or higher.
- `bun run build` succeeds from `frontend/`.
- No later-branch feature or E2E responsibility is absorbed.
- Scope, verification, and unfinished work are recorded before integration into `feat/mojica-mvp-ui`.

## Unresolved decisions and blockers

- Confirm the exact shadcn primitives needed by the component compositions before generation; avoid adding unused vendor primitives.
- Confirm final focus, hover, disabled, and pressed-state values against the approved Figma states before introducing component-local classes.
- The Storybook and accessibility checks are available in package scripts but cannot pass until the branch adds its stories.
