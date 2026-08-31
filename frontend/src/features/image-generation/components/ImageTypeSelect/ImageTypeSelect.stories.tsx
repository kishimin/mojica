import type { Meta, StoryObj } from "@storybook/react-vite";
import { useState } from "react";
import { vi } from "vitest";
import ImageTypeSelect from "./ImageTypeSelect";

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
};

export const KeyboardSelection: Story = {
  args: {
    onChange: vi.fn<(value: string) => void>(),
  },
  render: (args) => <ControlledImageTypeSelect {...args} />,
};

export const WithError: Story = {
  args: {
    onChange: vi.fn<(value: string) => void>(),
    errorMessage: "画像タイプを選択してください",
  },
  render: (args) => <ControlledImageTypeSelect {...args} />,
};
