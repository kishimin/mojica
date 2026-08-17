import { defineConfig } from 'orval'

// Input is a snapshot of the mojica API's OpenAPI document
// (`GET /swagger/v1/swagger.json` from backend/Mojica.Api). Regenerate it
// by running the backend locally and re-fetching that endpoint, then rerun
// `bunx orval` to refresh the generated client below.
export default defineConfig({
  mojicaApi: {
    input: './openapi.json',
    output: {
      mode: 'tags-split',
      target: './src/api/generated',
      schemas: './src/api/generated/model',
      client: 'fetch',
    },
  },
  mojicaApiZod: {
    input: './openapi.json',
    output: {
      mode: 'tags-split',
      target: './src/api/generated/zod',
      client: 'zod',
    },
  },
})
