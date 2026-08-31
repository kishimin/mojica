import { screen } from "@testing-library/react";
import { useState } from "react";
import { describe, expect, test, vi } from "vitest";
import ImageTypeSelect from "./ImageTypeSelect";
import { setupWithI18n } from "@/tests/test-utils";
import type { ImageType } from "@/types/image-type";

const ControlledImageTypeSelect = () => {
  const [value, setValue] = useState<ImageType>("standard");

  return <ImageTypeSelect value={value} onChange={setValue} />;
};

describe("ImageTypeSelect", () => {
  test("displays every supported image type option in order when opened", async () => {
    const { user } = setupWithI18n(
      <ImageTypeSelect
        value={"standard"}
        onChange={vi.fn<(value: string) => void>()}
      />,
    );

    await user.click(screen.getByRole("combobox"));

    const options = screen.getAllByRole("option");

    expect(options).toHaveLength(3);
    expect(options.map((option) => option.textContent)).toEqual([
      "標準画像",
      "X背景画像",
      "Xアイコン画像",
    ]);
  });

  test("displays the localized label for the controlled image type", () => {
    setupWithI18n(
      <ImageTypeSelect
        value={"standard"}
        onChange={vi.fn<(value: string) => void>()}
      />,
    );

    expect(screen.getByRole("combobox")).toHaveTextContent("標準画像");
  });

  test("changes the selected image type through keyboard interaction", async () => {
    const { user } = setupWithI18n(<ControlledImageTypeSelect />);

    await user.click(screen.getByRole("combobox"));
    await user.keyboard("{ArrowDown}{Enter}");

    expect(screen.getByRole("combobox")).toHaveTextContent("X背景画像");
  });

  test("associates the validation message with the selector", () => {
    setupWithI18n(
      <ImageTypeSelect
        value={"standard"}
        onChange={vi.fn<(value: string) => void>()}
        errorMessage={"画像タイプを選択してください"}
      />,
    );

    const selector = screen.getByRole("combobox", { name: "画像タイプ" });

    expect(screen.getByText("画像タイプを選択してください")).toBeVisible();
    expect(selector).toHaveAccessibleErrorMessage(
      "画像タイプを選択してください",
    );
  });

  test("keeps selector and validation IDs unique across instances", () => {
    setupWithI18n(
      <>
        <ImageTypeSelect
          value={"standard"}
          onChange={vi.fn<(value: string) => void>()}
          errorMessage={"画像タイプを選択してください"}
        />
        <ImageTypeSelect
          value={"x-icon"}
          onChange={vi.fn<(value: string) => void>()}
          errorMessage={"画像タイプを選択してください"}
        />
      </>,
    );

    const selectors = screen.getAllByRole("combobox", { name: "画像タイプ" });
    const errorMessageIds = selectors.map((selector) =>
      selector.getAttribute("aria-errormessage"),
    );

    expect(new Set(errorMessageIds).size).toBe(2);
    errorMessageIds.forEach((errorMessageId) => {
      expect(errorMessageId).not.toBeNull();
      expect(document.getElementById(errorMessageId as string)).toBeVisible();
    });
  });

  test("displays English labels when English is the active locale", async () => {
    const { user } = setupWithI18n(
      <ImageTypeSelect
        value={"standard"}
        onChange={vi.fn<(value: string) => void>()}
      />,
      "en",
    );

    await user.click(screen.getByRole("combobox", { name: "Image type" }));

    const options = screen.getAllByRole("option");

    expect(options).toHaveLength(3);
    expect(options.map((option) => option.textContent)).toEqual([
      "Standard image",
      "X background image",
      "X icon image",
    ]);
  });
});
