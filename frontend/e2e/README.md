# Reproducing CI browser tests

CI builds `frontend/Dockerfile.e2e` for the push, pull-request, and nightly
workflows. The image pins Playwright's OS and fonts, Bun, Google Chrome, and
Microsoft Edge. `fonts.conf` selects Noto Sans CJK JP for generic sans-serif
families instead of relying on the runner's Japanese fallback fonts.
Keep the Playwright image version aligned with `bun.lock`.
Do not use the hosted runner's browsers to regenerate Linux visual baselines.

From the repository root on a Linux Docker host, with Mojica API and Glyph Forge
running as configured in the workflows:

```sh
docker build --file frontend/Dockerfile.e2e --tag mojica-e2e frontend
docker run --name mojica-e2e --init --ipc=host --network host \
  --env VITE_API_URL=http://127.0.0.1:5063 mojica-e2e
docker cp mojica-e2e:/app/playwright-report frontend/playwright-report
docker cp mojica-e2e:/app/test-results frontend/test-results
docker rm mojica-e2e
```

To regenerate visual baselines after reviewing an intentional rendering change,
replace the run command with:

```sh
docker run --name mojica-e2e --init --ipc=host --network host \
  --env VITE_API_URL=http://127.0.0.1:5063 mojica-e2e \
  bash -c 'bun run build && bun run e2e --update-snapshots'
docker cp mojica-e2e:/app/e2e/tests/error-fallback.visual.medium.test.ts-snapshots/. frontend/e2e/tests/error-fallback.visual.medium.test.ts-snapshots/
docker cp mojica-e2e:/app/e2e/tests/visual-regression.medium.test.ts-snapshots/. frontend/e2e/tests/visual-regression.medium.test.ts-snapshots/
```

Review the changed images, rebuild the image to include them, and run again
without `--update-snapshots` before committing. Release tests require
`RELEASE_E2E_BASE_URL` and remain separate from this local-service run.
