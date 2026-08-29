import { useQueryClient } from "@tanstack/react-query";
import { renderHook } from "@testing-library/react";
import { describe, expect, test } from "vitest";
import { useI18n } from "../../hooks/use-i18n";
import { queryClient } from "../../lib/react-query";
import { AppProviders } from "./AppProviders";

describe("AppProviders", () => {
  test("provides the shared query client and locale", () => {
    const { result } = renderHook(
      () => ({
        locale: useI18n().locale,
        queryClient: useQueryClient(),
      }),
      { wrapper: AppProviders },
    );

    expect(result.current.queryClient).toBe(queryClient);
    expect(result.current.locale).toBe("ja");
  });
});
