import type { Meta, StoryObj } from "@storybook/react-vite";

import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "./select";

const meta = {
  title: "UI/Select",
  component: Select,
} satisfies Meta<typeof Select>;

export default meta;

type Story = StoryObj<typeof meta>;

const options = ["Portrait", "Landscape", "Square"] as const;

const renderSelect = (disabled = false) => (
  <Select defaultValue="Portrait">
    <SelectTrigger aria-label="Image type" disabled={disabled}>
      <SelectValue />
    </SelectTrigger>
    <SelectContent>
      {options.map((option) => (
        <SelectItem key={option} value={option}>
          {option}
        </SelectItem>
      ))}
    </SelectContent>
  </Select>
);

export const Default: Story = { render: () => renderSelect() };

export const Disabled: Story = { render: () => renderSelect(true) };
