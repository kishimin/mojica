import type { Meta, StoryObj } from "@storybook/react-vite";
import { I18nProvider } from "../../providers/I18nProvider";
import AppHeader from "./AppHeader";

const meta = {
  title: "App/AppHeader",
  component: AppHeader,
  decorators: [
    (Story) => (
      <I18nProvider>
        <Story />
      </I18nProvider>
    ),
  ],
} satisfies Meta<typeof AppHeader>;

export default meta;

type Story = StoryObj<typeof meta>;

export const Default: Story = {};
