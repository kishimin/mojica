import type { Meta, StoryObj } from "@storybook/react-vite";
import ErrorFallback from "./ErrorFallback";

const meta = {
  title: "Features/Error/ErrorFallback",
  component: ErrorFallback,
  parameters: {
    docs: {
      description: {
        component:
          "Root error recovery screen with provider-independent localized copy.",
      },
    },
  },
} satisfies Meta<typeof ErrorFallback>;

export default meta;

type Story = StoryObj<typeof meta>;

export const Default: Story = {};
