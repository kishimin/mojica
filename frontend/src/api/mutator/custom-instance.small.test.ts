import { HttpResponse, http } from "msw";
import { describe, expect, test } from "vitest";
import { worker } from "../mocks/browser";
import { customInstance } from "./custom-instance";

describe("customInstance HTTP contract", () => {
  test("returns the Axios response for a successful request", async () => {
    worker.use(
      http.get("*/custom-instance/success", () =>
        HttpResponse.json({ message: "ok" }),
      ),
    );

    const result = await customInstance<{ message: string }>({
      method: "GET",
      url: "/custom-instance/success",
    });

    expect(result.data).toEqual({ message: "ok" });
    expect(result.status).toBe(200);
  });

  test("sends the request method, URL parameters, and body", async () => {
    const receivedRequests: Array<{
      method: string;
      url: string;
      body: unknown;
    }> = [];

    worker.use(
      http.post("*/custom-instance/echo", async ({ request }) => {
        receivedRequests.push({
          method: request.method,
          url: request.url,
          body: await request.json(),
        });

        return HttpResponse.json({ accepted: true });
      }),
    );

    await customInstance({
      method: "POST",
      url: "/custom-instance/echo",
      params: { locale: "ja" },
      data: { name: "mojica" },
    });

    expect(receivedRequests).toEqual([
      {
        method: "POST",
        url: "http://localhost:5063/custom-instance/echo?locale=ja",
        body: { name: "mojica" },
      },
    ]);
  });

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
