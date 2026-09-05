import type { Meta, StoryObj } from "@storybook/react-vite";
import ErrorFallback from "./ErrorFallback";
import type { Locale } from "@/types/i18n";

const meta = {
  title: "Features/Error/ErrorFallback",
  component: ErrorFallback,
  parameters: {
    docs: {
      description: {
        component:
          "Root error recovery screen with provider-independent localized copy.",
      },
    },
  },
} satisfies Meta<typeof ErrorFallback>;

export default meta;

type Story = StoryObj<typeof meta>;

const storyWithLocale = (locale: Locale): Story => ({
  beforeEach: () => {
    localStorage.setItem("locale", locale);

    return () => {
      localStorage.removeItem("locale");
    };
  },
});

export const Japanese: Story = storyWithLocale("ja");

export const English: Story = storyWithLocale("en");
