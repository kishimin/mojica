import type { Meta, StoryObj } from "@storybook/react-vite";
import { useState } from "react";
import { expect, userEvent, within } from "storybook/test";
import { vi } from "vitest";
import ImageTypeSelect from "./ImageTypeSelect";
import { I18nProvider } from "@/providers/I18nProvider";

const meta = {
  title: "Features/ImageGeneration/ImageTypeSelect",
  component: ImageTypeSelect,
  parameters: {
    docs: {
      description: {
        component:
          "Controlled selector for choosing the image type sent to the image generation API.",
      },
    },
  },
  args: {
    value: "standard",
  },
  decorators: [
    (Story) => (
      <I18nProvider>
        <Story />
      </I18nProvider>
    ),
  ],
} satisfies Meta<typeof ImageTypeSelect>;

export default meta;

type Story = StoryObj<typeof meta>;

const ControlledImageTypeSelect = (args: Story["args"]) => {
  const [value, setValue] = useState(args?.value ?? "standard");

  return <ImageTypeSelect {...args} value={value} onChange={setValue} />;
};

export const Default: Story = {
  args: {
    onChange: vi.fn<(value: string) => void>(),
  },
  render: (args) => <ControlledImageTypeSelect {...args} />,
  play: async ({ canvasElement }) => {
    const canvas = within(canvasElement);
    const selector = canvas.getByRole("combobox", { name: "画像タイプ" });

    await userEvent.click(selector);

    const options = within(document.body).getAllByRole("option");

    await expect(options).toHaveLength(3);
    await expect(options[0]).toHaveTextContent("標準画像");
    await expect(options[1]).toHaveTextContent("X背景画像");
    await expect(options[2]).toHaveTextContent("Xアイコン画像");
  },
};

export const KeyboardSelection: Story = {
  args: {
    onChange: vi.fn<(value: string) => void>(),
  },
  render: (args) => <ControlledImageTypeSelect {...args} />,
  play: async ({ canvasElement }) => {
    const canvas = within(canvasElement);
    const selector = canvas.getByRole("combobox", { name: "画像タイプ" });

    await userEvent.click(selector);
    await userEvent.keyboard("{ArrowDown}{Enter}");

    await expect(selector).toHaveTextContent("X背景画像");
  },
};

export const WithError: Story = {
  args: {
    onChange: vi.fn<(value: string) => void>(),
    errorMessage: "画像タイプを選択してください",
  },
  render: (args) => <ControlledImageTypeSelect {...args} />,
};
