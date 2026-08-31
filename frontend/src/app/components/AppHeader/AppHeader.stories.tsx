import type { Meta, StoryObj } from "@storybook/react-vite";
import AppHeader from "./AppHeader";

const meta = {
  title: "App/AppHeader",
  component: AppHeader,
  parameters: {
    docs: {
      description: {
        component:
          "Application header combining the Mojica logo and locale switcher.",
      },
    },
  },
} satisfies Meta<typeof AppHeader>;

export default meta;

type Story = StoryObj<typeof meta>;

export const Default: Story = {};
