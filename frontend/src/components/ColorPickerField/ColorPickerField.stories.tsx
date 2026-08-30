import type { Meta, StoryObj } from "@storybook/react-vite";
import { useState } from "react";
import ColorPickerField from "./ColorPickerField";

const meta = {
  title: "Components/ColorPickerField",
  component: ColorPickerField,
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
    onChange: () => undefined,
  },
  render: (args) => <ControlledColorPickerField {...args} />,
};

export const WithError: Story = {
  args: {
    onChange: () => undefined,
    errorMessage: "Enter a valid HEX color",
  },
  render: (args) => <ControlledColorPickerField {...args} />,
};

export const Disabled: Story = {
  args: {
    onChange: () => undefined,
    disabled: true,
  },
  render: (args) => <ControlledColorPickerField {...args} />,
};
