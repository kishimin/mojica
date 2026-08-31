import type { Meta, StoryObj } from "@storybook/react-vite";
import { useState } from "react";
import { vi } from "vitest";
import ColorPickerField from "./ColorPickerField";

const meta = {
  title: "Components/ColorPickerField",
  component: ColorPickerField,
  parameters: {
    docs: {
      description: {
        component: "Combined native color picker and editable HEX text field.",
      },
    },
  },
  args: {
    label: "Color",
    colorPickerLabel: "Choose color",
    value: "#FFD400",
  },
} satisfies Meta<typeof ColorPickerField>;

export default meta;

type Story = StoryObj<typeof meta>;

const ControlledColorPickerField = (args: Story["args"]) => {
  const [value, setValue] = useState(args?.value ?? "#FFD400");

  return (
    <ColorPickerField
      {...args}
      label={args?.label ?? "Color"}
      colorPickerLabel={args?.colorPickerLabel ?? "Choose color"}
      value={value}
      onChange={setValue}
    />
  );
};

export const Default: Story = {
  args: {
    onChange: vi.fn<(hex: string) => void>(),
  },
  render: (args) => <ControlledColorPickerField {...args} />,
};

export const WithError: Story = {
  args: {
    onChange: vi.fn<(hex: string) => void>(),
    errorMessage: "Enter a valid HEX color",
  },
  render: (args) => <ControlledColorPickerField {...args} />,
};

export const Disabled: Story = {
  args: {
    onChange: vi.fn<(hex: string) => void>(),
    disabled: true,
  },
  render: (args) => <ControlledColorPickerField {...args} />,
};
