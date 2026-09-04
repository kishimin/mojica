import type { Meta, StoryObj } from "@storybook/react-vite";
import ImageGenerationScreen from "./ImageGenerationScreen";
import { I18nProvider } from "@/providers/I18nProvider";

const meta = {
  title: "Features/Image Generation/ImageGenerationScreen",
  component: ImageGenerationScreen,
  parameters: {
    docs: {
      description: {
        component:
          "Localized page body that presents the image-generation heading, description, and form.",
      },
    },
  },
} satisfies Meta<typeof ImageGenerationScreen>;

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
