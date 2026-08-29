import { useQueryClient } from "@tanstack/react-query";
import { render, screen } from "@testing-library/react";
import { describe, expect, test } from "vitest";
import { useI18n } from "../../hooks/use-i18n";
import { queryClient } from "../../lib/react-query";
import { AppProviders } from "./AppProviders";

const ProviderConsumer = () => {
  const { locale } = useI18n();
  const activeQueryClient = useQueryClient();

  return (
    <output>
      {activeQueryClient === queryClient ? locale : "unexpected query client"}
    </output>
  );
};

describe("AppProviders", () => {
  test("provides the shared query client and locale", () => {
    render(
      <AppProviders>
        <ProviderConsumer />
      </AppProviders>,
    );

    expect(screen.getByText("ja")).toBeInTheDocument();
  });
});
