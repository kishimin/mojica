import { describe, test } from "vitest";

describe("mojica API client contract", () => {
  // ID: FOUNDATION-API-S-001
  // Source: docs/v1/ui/frontend-architecture.md §Environment configuration
  // Given: VITE_API_URL identifies the mojica API origin
  // When: the API client sends a request with a relative endpoint path
  // Then: the request targets that configured API origin
  // Blocked by: injectable environment configuration at the API client boundary
  // Priority: P0
  test.todo("resolves relative endpoints against VITE_API_URL");

  // ID: FOUNDATION-API-S-002
  // Source: docs/v1/ui/ui.md §3 Header and §13 Internationalization
  // Given: Japanese is the active UI locale
  // When: the API client sends a request
  // Then: the request includes Accept-Language with the value ja
  // Blocked by: locale-aware API client wiring
  // Priority: P0
  test.todo("sends ja as Accept-Language for the Japanese locale");

  // ID: FOUNDATION-API-S-003
  // Source: docs/v1/ui/ui.md §3 Header and §13 Internationalization
  // Given: English is the active UI locale
  // When: the API client sends a request
  // Then: the request includes Accept-Language with the value en
  // Blocked by: locale-aware API client wiring
  // Priority: P0
  test.todo("sends en as Accept-Language for the English locale");

  // ID: FOUNDATION-API-S-004
  // Source: docs/v1/ui/frontend-architecture.md §API communication
  // Given: the API responds with a data payload
  // When: the custom Orval mutator resolves the response
  // Then: callers receive the response data rather than the Axios response wrapper
  // Blocked by: controllable Axios adapter at the mutator boundary
  // Priority: P1
  test.todo("returns the API response data to generated Orval clients");
});
