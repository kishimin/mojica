import type { Meta, StoryObj } from "@storybook/react-vite";
import ImageUploadForm from "./ImageUploadForm";
import { I18nProvider } from "@/providers/I18nProvider";

const meta = {
  title: "Features/Image Generation/ImageUploadForm",
  component: ImageUploadForm,
  render: (args) => (
    <I18nProvider initialLocale={args.locale}>
      <ImageUploadForm {...args} />
    </I18nProvider>
  ),
} satisfies Meta<typeof ImageUploadForm>;

export default meta;
type Story = StoryObj<typeof meta>;

export const Japanese: Story = { args: { locale: "ja" } };
export const English: Story = { args: { locale: "en" } };
