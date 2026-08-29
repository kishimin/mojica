import { HttpResponse, http } from "msw";
import { describe, expect, test } from "vitest";
import { worker } from "../mocks/browser";
import { customInstance } from "./custom-instance";

describe("customInstance HTTP contract", () => {
  // ID: CUSTOM-INSTANCE-S-001
  // Source: ADR-0032; customInstance public transport contract
  // Given: The API returns a successful JSON response
  // When: A request is executed through customInstance
  // Then: The resolved value is the response body
  // Priority: P0
  test("returns the response body for a successful request", async () => {
    worker.use(
      http.get("*/custom-instance/success", () =>
        HttpResponse.json({ message: "ok" }),
      ),
    );

    const result = await customInstance<{ message: string }>({
      method: "GET",
      url: "/custom-instance/success",
    });

    expect(result).toEqual({ message: "ok" });
  });

  // ID: CUSTOM-INSTANCE-S-002
  // Source: ADR-0032; customInstance public transport contract
  // Given: A request has a method, URL, query, and JSON body
  // When: The request is executed through customInstance
  // Then: The API receives those request values
  // Priority: P0
  test("sends the request method, URL parameters, and body", async () => {
    let receivedRequest: {
      method: string;
      url: string;
      body: unknown;
    } | null = null;

    worker.use(
      http.post("*/custom-instance/echo", async ({ request }) => {
        receivedRequest = {
          method: request.method,
          url: request.url,
          body: await request.json(),
        };

        return HttpResponse.json({ accepted: true });
      }),
    );

    await customInstance({
      method: "POST",
      url: "/custom-instance/echo",
      params: { locale: "ja" },
      data: { name: "mojica" },
    });

    expect(receivedRequest).toEqual({
      method: "POST",
      url: "http://localhost:5063/custom-instance/echo?locale=ja",
      body: { name: "mojica" },
    });
  });

  // ID: CUSTOM-INSTANCE-S-003
  // Source: ADR-0032; customInstance public transport contract
  // Given: The API returns a non-success HTTP response
  // When: The request is executed through customInstance
  // Then: The returned promise rejects with the transport error
  // Error: non-2xx response
  // Priority: P0
  test("rejects when the API returns a non-success response", async () => {
    worker.use(
      http.get("*/custom-instance/error", () =>
        HttpResponse.json({ error: "invalid request" }, { status: 400 }),
      ),
    );

    await expect(
      customInstance({
        method: "GET",
        url: "/custom-instance/error",
      }),
    ).rejects.toMatchObject({
      response: { status: 400 },
    });
  });

  // ID: CUSTOM-INSTANCE-S-004
  // Source: frontend runtime API boundary
  // Given: The API cannot be reached
  // When: The request is executed through customInstance
  // Then: The returned promise rejects with a network error
  // Error: network failure
  // Priority: P1
  test("rejects when the API cannot be reached", async () => {
    worker.use(
      http.get("*/custom-instance/network-error", () => HttpResponse.error()),
    );

    await expect(
      customInstance({
        method: "GET",
        url: "/custom-instance/network-error",
      }),
    ).rejects.toBeTruthy();
  });
});
