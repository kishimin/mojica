import type { Meta, StoryObj } from "@storybook/react-vite";
import TextField from "./TextField";

const meta = {
  title: "Components/TextField",
  component: TextField,
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
