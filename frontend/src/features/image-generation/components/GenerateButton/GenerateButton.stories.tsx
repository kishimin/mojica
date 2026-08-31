import type { Meta, StoryObj } from "@storybook/react-vite";
import GenerateButton from "./GenerateButton";
import { I18nContext } from "@/hooks/i18n-context";

const meta = {
  title: "Features/Image Generation/GenerateButton",
  component: GenerateButton,
  parameters: {
    docs: {
      description: {
        component:
          "Primary action for image generation, including submitting, retryable, and retry cooldown states.",
      },
    },
  },
  decorators: [
    (Story) => (
      <I18nContext.Provider
        value={{ locale: "ja", setLocale: () => undefined }}
      >
        <Story />
      </I18nContext.Provider>
    ),
  ],
} satisfies Meta<typeof GenerateButton>;

export default meta;

type Story = StoryObj<typeof meta>;

export const Idle: Story = {
  args: {
    state: { kind: "idle" },
  },
};

export const Submitting: Story = {
  args: {
    state: { kind: "submitting" },
  },
};

export const Retryable: Story = {
  args: {
    state: { kind: "retryable" },
  },
};

export const Cooldown: Story = {
  args: {
    state: { kind: "cooldown", remainingSeconds: 5 },
  },
};
