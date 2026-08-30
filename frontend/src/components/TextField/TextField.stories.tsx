import type { Meta, StoryObj } from "@storybook/react-vite";

import TextField from "./TextField";

const meta = {
  title: "Components/TextField",
  component: TextField,
} satisfies Meta<typeof TextField>;

export default meta;

type Story = StoryObj<typeof meta>;

export const Default: Story = {
  args: {
    label: "Prompt",
    placeholder: "Describe the image",
  },
};

export const Filled: Story = {
  args: {
    label: "Prompt",
    defaultValue: "A sunset over the sea",
  },
};

export const Error: Story = {
  args: {
    label: "Prompt",
    errorMessage: "Prompt is required",
  },
};

export const Disabled: Story = {
  args: {
    label: "Prompt",
    disabled: true,
    defaultValue: "Unavailable",
  },
};
