import { defineConfig } from "orval";

export default defineConfig({
  petstore: {
    output: {
      mode: "tags-split",
      target: "src/api/endpoints/petstore.ts",
      schemas: "src/models",
      client: "react-query",
      httpClient: "axios",
      mock: true,
      clean: true,
      formatter: "prettier",
      override: {
        mutator: {
          path: "src/api/mutator/custom-instance.ts",
          name: "customInstance",
        },
      },
    },
    input: {
      target: "http://localhost:18080/swagger/v1/swagger.json",
    },
  },
  petstoreZod: {
    output: {
      mode: "tags-split",
      client: "zod",
      target: "src/gen/endpoints",
      fileExtension: ".zod.ts",
      formatter: "prettier",
    },
    input: {
      target: "http://localhost:18080/swagger/v1/swagger.json",
    },
  },
});
