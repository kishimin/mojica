import type { Meta, StoryObj } from "@storybook/react-vite";
import TextField from "./TextField";

const meta = {
  title: "Components/TextField",
  component: TextField,
  parameters: {
    docs: {
      description: {
        component: "Labeled text input with accessible validation feedback.",
      },
    },
  },
  args: {
    label: "Name",
  },
} satisfies Meta<typeof TextField>;

export default meta;

type Story = StoryObj<typeof meta>;

export const Default: Story = {};

export const WithPlaceholder: Story = {
  args: {
    placeholder: "Enter your name",
  },
};

export const WithError: Story = {
  args: {
    errorMessage: "Name is required",
  },
};

export const LongValue: Story = {
  args: {
    defaultValue:
      "This is a long value that verifies the text field remains usable when the entered text exceeds the available width.",
  },
};
