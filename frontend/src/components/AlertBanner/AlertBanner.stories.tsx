import type { Meta, StoryObj } from "@storybook/react-vite";
import AlertBanner from "./AlertBanner";

const meta = {
  title: "Components/AlertBanner",
  component: AlertBanner,
  parameters: {
    docs: {
      description: {
        component:
          "Destructive alert for communicating image-generation failures.",
      },
    },
  },
} satisfies Meta<typeof AlertBanner>;

export default meta;

type Story = StoryObj<typeof meta>;

export const Default: Story = {
  args: {
    title: "Image generation failed",
    description: "Please try again later.",
  },
};

export const LongMessage: Story = {
  args: {
    title: "Image generation failed",
    description:
      "The image service could not complete the request. Check your connection and try again later.",
  },
};
