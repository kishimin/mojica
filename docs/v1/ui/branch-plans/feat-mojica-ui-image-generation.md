# feat/mojica-ui-image-generation branch plan

## Source

- Implementation plan: [`../implementation-plan.md`](../implementation-plan.md)
- UI specification: [`../ui.md`](../ui.md)
- Component design: [`../component-design.md`](../component-design.md)
- Form contract: [`../components/ImageGenerationForm.md`](../components/ImageGenerationForm.md)
- Screen contract: [`../components/ImageGenerationScreen.md`](../components/ImageGenerationScreen.md)
- API contract: [`../../api/api.md`](../../api/api.md)

## Start point and prerequisites

- Branch: `feat/mojica-ui-image-generation`
- Start point: `main` at `7a259ab` (the foundation, shared components, and ESLint follow-up are merged)
- Prerequisites: the shared controls, `I18nProvider`, Orval-generated API client, and TanStack Query wiring are available on `main`.
- Dependencies: `ImageGenerationForm` consumes `ImageTypeSelect`, `ColorPickerField`, `TextField`, `FieldError`, `AlertBanner`, and `GenerateButton` from the shared/component work.

## Owned scope

This branch owns the image-generation screen and its feature-local behavior:

- `ImageGenerationForm` composition and form submission
- Client validation using the Zod schema and React Hook Form
- `POST /images` through the generated mutation hook
- Field mapping for API `422 errors[].field`
- API error presentation for `400`, `429`, `500`, `502`, and `504`
- `Retry-After` countdown behavior
- Automatic PNG download using `Content-Disposition`
- `ImageGenerationScreen` page composition and Storybook states

## Ordered work items

The decomposition below is an implementation plan inferred from the contracts; it does not add product behavior beyond the linked specifications.

- [ ] Add `imageGenerationSchema.ts` with every validation rule in `ui.md §11`, including length, required, whitespace, control-character, and character-combination rules.
- [ ] Add Small tests for every schema rule, then implement the schema until the tests pass.
- [ ] Add `useImageGenerationForm.ts` with React Hook Form resolver wiring and documented default values; cover it with Small tests.
- [ ] Add `useRetryAfterCountdown.ts` with one-second decrement, zero stop, input restart, and unmount disposal; cover it with fake-timer Small tests.
- [ ] Add `toImageGenerationErrorPresentation.ts` mapping API error `code` values to localized headings, including the unsupported-code fallback; cover every mapping with Small tests.
- [ ] Implement `ImageGenerationForm.tsx` using the generated mutation hook and shared controls.
- [ ] Add the Small form integration test with MSW for success, `422`, `400`, `429`, `500`, `502`, and `504`, including field errors and retry behavior.
- [ ] Implement automatic PNG download from the successful response body and `Content-Disposition` filename.
- [ ] Implement `ImageGenerationScreen.tsx` as the page container with the documented centered maximum width.
- [ ] Add the `ImageGenerationForm` and `ImageGenerationScreen` Storybook stories with MSW-backed Default, Filled, ValidationError, ServerError, Submitting, Success, and API error states.
- [ ] Run formatting, type checking, lint, Small/Medium tests, coverage, Storybook tests/build, and frontend build before opening the PR.

## Non-goals

- [ ] Layout, AppHeader, AppFooter, and shared component changes owned by the component branch.
- [ ] 404 and unexpected-error pages or route wiring owned by `feat/mojica-ui-error-pages`.
- [ ] Playwright E2E, VRT, deployed API, and file-system download verification owned by `test/mojica-ui-e2e`.
- [ ] Generated Orval client edits, backend API changes, image preview, history, or server-side storage.

## Completion criteria

- Form behavior agrees with `ImageGenerationForm.md` and `ui.md`.
- `POST /images` is exercised through the generated mutation hook with MSW in tests and stories; no real API is contacted.
- Validation, field error association, localized API banners, retry countdown, and automatic download are covered by behavior-focused tests.
- `ImageGenerationScreen` composes the form without introducing an initial data fetch or nested ErrorBoundary.
- Available repository verification commands pass: `bunx prettier --check`, `bun run typecheck`, `bun run lint`, `bun run test:small`, `bun run test:medium`, `bun run test:coverage:pr`, `bun run test:storybook`, `bun run build-storybook`, and `bun run build`.
- Every coverage metric in the PR summary is at least 80%.

## Unresolved decisions and blockers

- No blocker is known from the source contracts.
- Exact localized wording for new validation and error headings must follow the existing i18n dictionary conventions; do not invent English or Japanese copy outside the specification.
- Download filename parsing and browser compatibility remain covered by the later E2E branch, while this branch must still test the form's success callback behavior.
