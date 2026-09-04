import type { Meta, StoryObj } from "@storybook/react-vite";
import type { ComponentProps } from "react";
import { useArgs } from "storybook/preview-api";
import { fn } from "storybook/test";
import ImageTypeSelect from "./ImageTypeSelect";
import { I18nProvider } from "@/providers/I18nProvider";
import type { ImageType } from "@/types/image-type";

type ImageTypeSelectStoryArgs = ComponentProps<typeof ImageTypeSelect>;

const RenderImageTypeSelect = (args: ImageTypeSelectStoryArgs) => {
  const [{ value }, updateArgs] = useArgs<{ value: ImageType }>();

  return (
    <ImageTypeSelect
      {...args}
      value={value}
      onChange={(nextValue) => updateArgs({ value: nextValue })}
    />
  );
};

const meta = {
  title: "Features/ImageGeneration/ImageTypeSelect",
  component: ImageTypeSelect,
  parameters: {
    docs: {
      description: {
        component:
          "Controlled selector for choosing the image type sent to the image generation API.",
      },
    },
  },
  args: {
    value: "standard",
  },
  render: RenderImageTypeSelect,
} satisfies Meta<typeof ImageTypeSelect>;

export default meta;

type Story = StoryObj<typeof meta>;

export const Default: Story = {
  args: {
    onChange: fn(),
  },
};

export const KeyboardSelection: Story = {
  args: {
    onChange: fn(),
  },
};

export const WithError: Story = {
  args: {
    onChange: fn(),
    errorMessage: "画像タイプを選択してください",
  },
};

export const English: Story = {
  args: {
    onChange: fn(),
  },
  decorators: [
    (Story) => (
      <I18nProvider initialLocale={"en"}>
        <Story />
      </I18nProvider>
    ),
  ],
};
