# `test/mojica-ui-e2e` Work Plan

## Branch and sources

- Branch: `test/mojica-ui-e2e`
- Start point: `origin/main` at `ac38d0f` (image-generation and error-page work are merged)
- Implementation plan: [`../implementation-plan.md`](../implementation-plan.md)
- UI specification: [`../ui.md`](../ui.md)
- Component design: [`../component-design.md`](../component-design.md)
- Playwright configuration: [`../../../frontend/playwright.config.ts`](../../../frontend/playwright.config.ts)

## Prerequisites and dependencies

- The shared foundation, UI components, image-generation flow, error pages, and root routing are available on `main`.
- Tests must use the repository's Playwright projects and a configured real Mojica API.
- Do not replace the Mojica API with `page.route`, MSW, or another frontend response mock in an E2E test.
- A deployed API or an explicitly configured local API/Glyph Forge environment is required before the image-generation flow can be added.

## Owned scope

This branch owns browser-level verification of the completed UI:

- Real-browser image-generation flow through a configured real API.
- PNG download behavior and filename verification.
- Responsive rendering at the documented viewport sizes.
- 404 navigation and unexpected-error recovery screens.
- Keyboard interaction and visual regression baselines.

## Ordered work items

The decomposition below is inferred from the implementation plan and linked specifications; it does not add product behavior.

- [x] Inspect the existing Playwright projects, fixtures, server lifecycle, and test data boundaries.
- [ ] Confirm the real Mojica API and Glyph Forge environment used by E2E runs.
- [ ] Add a browser test for the image-generation happy path after the real API environment is available.
- [ ] Add a browser test for validation and documented API error presentations where browser behavior adds value beyond Small/Medium tests.
- [ ] Add PNG download verification with the file-system dependency classified as at least Medium.
- [ ] Add responsive checks at 390px, 768px, and 1440px without creating viewport-specific duplicate stories.
- [ ] Add 404 navigation from an unknown route back to the image-generation home.
- [ ] Add unexpected-error fallback recovery behavior at the browser boundary.
- [ ] Add keyboard interaction coverage for the user-facing controls.
- [ ] Add or update visual regression baselines and run them across every configured Playwright project.
- [ ] Classify each test by actual dependencies and encode the size in its filename or project convention.
- [ ] Run the repository-defined formatting, typecheck, lint, unit/size tests, Storybook checks, build, and Playwright verification before opening the PR.

## Non-goals

- [ ] Reimplementing component, form, routing, or error-page behavior already owned by earlier branches.
- [ ] Replacing Small/Medium unit and integration tests with browser tests.
- [ ] Changing API contracts, generated clients, backend behavior, or design tokens.
- [ ] Replacing the real API with a frontend network mock.
- [ ] Contacting production services from default local CI runs.

## Completion criteria

- Browser tests assert user-observable behavior and pass in every configured Playwright project.
- Test size reflects actual network, file-system, browser, and deployment dependencies.
- Visual baselines are reviewed and stable at the documented viewport widths.
- Existing checks remain green: `bunx prettier --check`, `bun run typecheck`, `bun run lint`, `bun run test:small`, `bun run test:medium`, `bun run test:large`, `bun run test:coverage:pr`, `bun run test:storybook`, `bun run build-storybook`, `bun run build`, and `bun run e2e` when the required local services are available.
- Coverage remains at least 80% for every reported metric where the repository collects it.

## Unresolved decisions and blockers

- The current Playwright web server starts only the frontend preview; no E2E API/Glyph Forge service lifecycle is configured.
- Confirm the non-production Mojica API and Glyph Forge endpoints before adding real image-generation coverage.
- Confirm which Playwright project owns VRT and download tests; do not assume all browser tests are Large.
- No implementation work is included until the test boundaries and fixtures are confirmed.
