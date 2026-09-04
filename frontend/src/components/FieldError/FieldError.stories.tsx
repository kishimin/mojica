import type { Meta, StoryObj } from "@storybook/react-vite";
import FieldError from "./FieldError";

const meta = {
  title: "Components/FieldError",
  component: FieldError,
  parameters: {
    docs: {
      description: {
        component: "Validation message associated with a form control.",
      },
    },
  },
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

export const LongMessage: Story = {
  args: {
    message:
      "This validation message is intentionally long to verify that it wraps within the available field width.",
  },
};
