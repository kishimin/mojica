import type { Meta, StoryObj } from "@storybook/react-vite";
import { useState } from "react";
import LanguageSwitcher from "./LanguageSwitcher";

const meta = {
  title: "Components/LanguageSwitcher",
  component: LanguageSwitcher,
} satisfies Meta<typeof LanguageSwitcher>;

export default meta;

type Story = StoryObj<typeof meta>;

const ControlledLanguageSwitcher = (args: Story["args"]) => {
  const [locale, setLocale] = useState(args?.locale ?? "ja");

  return <LanguageSwitcher {...args} locale={locale} onChange={setLocale} />;
};

export const Default: Story = {
  args: {
    locale: "ja",
    onChange: () => undefined,
  },
  render: (args) => <ControlledLanguageSwitcher {...args} />,
};

export const English: Story = {
  args: {
    locale: "en",
    onChange: () => undefined,
  },
  render: (args) => <ControlledLanguageSwitcher {...args} />,
};
