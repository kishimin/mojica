import type { Meta, StoryObj } from "@storybook/react-vite";
import FieldError from "./FieldError";

const meta = {
  title: "Components/FieldError",
  component: FieldError,
} satisfies Meta<typeof FieldError>;

export default meta;

type Story = StoryObj<typeof meta>;

export const Default: Story = {
  args: {
    message: "Name is required",
  },
};

export const Empty: Story = {
  args: {
    message: "",
  },
};
