import type { Meta, StoryObj } from "@storybook/react-vite";
import NotFoundView from "./NotFoundView";
import { I18nProvider } from "@/providers/I18nProvider";

const meta = {
  title: "Features/Not Found/NotFoundView",
  component: NotFoundView,
  parameters: {
    docs: {
      description: {
        component:
          "Localized 404 screen with an accessible link back to the home page.",
      },
    },
  },
} satisfies Meta<typeof NotFoundView>;

export default meta;

type Story = StoryObj<typeof meta>;

export const Default: Story = {};

export const English: Story = {
  decorators: [
    (Story) => (
      <I18nProvider initialLocale={"en"}>
        <Story />
      </I18nProvider>
    ),
  ],
};
