import type { Meta, StoryObj } from "@storybook/react-vite";
import ImageGenerationForm from "./ImageGenerationForm";
import { I18nProvider } from "@/providers/I18nProvider";

const meta = {
  title: "Features/Image Generation/ImageGenerationForm",
  component: ImageGenerationForm,
  parameters: {
    docs: {
      description: {
        component:
          "Image-generation form with localized fields, validation feedback, API error handling, and PNG download behavior.",
      },
    },
  },
  render: (args) => (
    <I18nProvider initialLocale={args.locale}>
      <ImageGenerationForm {...args} />
    </I18nProvider>
  ),
} satisfies Meta<typeof ImageGenerationForm>;

export default meta;

type Story = StoryObj<typeof meta>;

export const Default: Story = {
  args: {
    locale: "ja",
  },
};

export const English: Story = {
  args: {
    locale: "en",
  },
};
