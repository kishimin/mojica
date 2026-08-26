# mojica UI Branch Implementation Plan

Design documents in scope:

- [`ui.md`](./ui.md)
- [`component-design.md`](./component-design.md)
- [`design-tokens.md`](./design-tokens.md)

## 1. Branch structure

Do not implement directly on a shared branch. Use one parent branch for the entire MVP UI and child branches for each work unit.

```text
main
└── feat/mojica-mvp-ui
    ├── feat/mojica-ui-foundation
    ├── feat/mojica-ui-components
    ├── feat/mojica-ui-image-generation
    ├── feat/mojica-ui-error-pages
    └── test/mojica-ui-e2e
```

### Parent branch

`feat/mojica-mvp-ui`

- Create from `main`.
- Merge child branches into it.
- Merge children in order and keep it buildable throughout.

### Child branch order

#### 1. `feat/mojica-ui-foundation`

Owns the foundation and design tokens:

- Material Design 3 CSS custom properties
- shared CSS and responsive foundation
- Axios, Orval, and TanStack Query wiring
- shared Zod schema placement
- minimal i18n and provider setup

Merge into `feat/mojica-mvp-ui` when complete.

#### 2. `feat/mojica-ui-components`

Owns shared UI components:

- `Logo`, `FieldError`, `TextField`, `ColorPickerField`, `AlertBanner`
- `LanguageSwitcher`, `ImageTypeSelect`, `GenerateButton`
- `AppHeader`, `AppFooter`, and `Layout`

Merge into the parent when complete.

#### 3. `feat/mojica-ui-image-generation`

Owns the image generation screen:

- form and client validation
- `POST /images`
- pending, success, 422, 400, 429, 500, 502, and 504 states
- `Retry-After` countdown
- automatic PNG download

Depends on the foundation and component branches. Merge into the parent when complete.

#### 4. `feat/mojica-ui-error-pages`

Owns error screens and route wiring:

- 404 screen and ErrorBoundary fallback
- route wiring and navigation from 404 to home
- normal browser reload from the error screen

Depends on the foundation and component branches. Merge into the parent when complete.

#### 5. `test/mojica-ui-e2e`

Owns E2E tests classified individually by actual dependency scope. Do not classify a test as Large merely because it uses Playwright or is called E2E. A test connecting to localhost frontend and API stubs is Medium; one connecting to a deployed, non-localhost API or real service is Large. A VRT or download test using the file system is at least Medium even without other dependencies.

- real-browser image generation flow
- `POST /images` through a localhost stub or deployed API
- PNG download, responsive display, 404 and error screens
- keyboard interaction and VRT

Depends on the image-generation and error-page branches. Merge after verification.

## 2. Merge order

```text
feat/mojica-ui-foundation
  ↓
feat/mojica-ui-components
  ↓
feat/mojica-ui-image-generation ─┐
                                 ├─→ feat/mojica-mvp-ui → main
feat/mojica-ui-error-pages ──────┘
                                 ↑
test/mojica-ui-e2e ──────────────┘
```

1. `feat/mojica-ui-foundation`
2. `feat/mojica-ui-components`
3. `feat/mojica-ui-image-generation`
4. `feat/mojica-ui-error-pages`
5. `test/mojica-ui-e2e`
6. Open a PR from `feat/mojica-mvp-ui` to `main`

## 3. Commit units

- `feat:` foundation, components, screens, and features
- `test:` Small, Medium, and Large tests
- `refactor:` organization without behavior changes
- `fix:` defects discovered during implementation
- `docs:` alignment between design and implementation

Do not mix responsibilities from multiple child branches in one commit. Update generated files in the same child branch as their generation source.

## 4. Completion criteria for every branch

- Design and implementation agree within the owned scope.
- Available formatting, tests, type checks, lint, and build pass.
- Every metric in the overall coverage summary is at least 80%.
- Classify each `test/mojica-ui-e2e` test as Small, Medium, or Large by actual dependencies and include the size in its filename. Before merging to the parent, confirm every size currently runnable.
- Each child PR records scope, verification, and unfinished work.
- Run all verification on the integrated parent before merging to `main`.
