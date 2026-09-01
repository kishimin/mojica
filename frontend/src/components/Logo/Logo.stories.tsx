import type { Meta, StoryObj } from "@storybook/react-vite";
import Logo from "./Logo";

const meta = {
  title: "Components/Logo",
  component: Logo,
  parameters: {
    docs: { description: { component: "Mojica brand mark and wordmark." } },
  },
} satisfies Meta<typeof Logo>;

export default meta;

type Story = StoryObj<typeof meta>;

export const Default: Story = {};
