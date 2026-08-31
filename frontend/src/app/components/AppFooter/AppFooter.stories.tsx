import type { Meta, StoryObj } from "@storybook/react-vite";
import AppFooter from "./AppFooter";

const meta = {
  title: "App/AppFooter",
  component: AppFooter,
  parameters: {
    docs: {
      description: {
        component: "Application footer with the site copyright notice.",
      },
    },
  },
} satisfies Meta<typeof AppFooter>;

export default meta;

type Story = StoryObj<typeof meta>;

export const Default: Story = {};
