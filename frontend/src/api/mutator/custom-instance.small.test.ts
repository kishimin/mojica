import { describe, test } from "vitest";

describe("customInstance HTTP contract", () => {
  // ID: CUSTOM-INSTANCE-S-001
  // Source: ADR-0032; customInstance public transport contract
  // Given: The API returns a successful JSON response
  // When: A request is executed through customInstance
  // Then: The resolved value is the response body
  // Priority: P0
  test.todo("returns the response body for a successful request");

  // ID: CUSTOM-INSTANCE-S-002
  // Source: ADR-0032; customInstance public transport contract
  // Given: A request has a method, URL, query, and JSON body
  // When: The request is executed through customInstance
  // Then: The API receives those request values
  // Priority: P0
  test.todo("sends the request method, URL parameters, and body");

  // ID: CUSTOM-INSTANCE-S-003
  // Source: ADR-0032; customInstance public transport contract
  // Given: The API returns a non-success HTTP response
  // When: The request is executed through customInstance
  // Then: The returned promise rejects with the transport error
  // Error: non-2xx response
  // Priority: P0
  test.todo("rejects when the API returns a non-success response");

  // ID: CUSTOM-INSTANCE-S-004
  // Source: frontend runtime API boundary
  // Given: The API cannot be reached
  // When: The request is executed through customInstance
  // Then: The returned promise rejects with a network error
  // Error: network failure
  // Priority: P1
  test.todo("rejects when the API cannot be reached");
});
