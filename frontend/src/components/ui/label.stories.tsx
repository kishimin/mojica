import type { Meta, StoryObj } from "@storybook/react-vite";

import { Input } from "./input";
import { Label } from "./label";

const meta = {
  title: "UI/Label",
  component: Label,
  args: { children: "Image prompt", htmlFor: "prompt" },
} satisfies Meta<typeof Label>;

export default meta;

type Story = StoryObj<typeof meta>;

export const Default: Story = {
  render: (args) => (
    <div className="grid w-80 gap-2">
      <Label {...args} />
      <Input id="prompt" />
    </div>
  ),
};
