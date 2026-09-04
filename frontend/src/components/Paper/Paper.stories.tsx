import type { Meta, StoryObj } from "@storybook/react-vite";
import Paper from "./Paper";

const meta = {
  title: "Components/Paper",
  component: Paper,
  parameters: {
    docs: {
      description: {
        component: "Raised surface for grouping related content.",
      },
    },
  },
} satisfies Meta<typeof Paper>;

export default meta;

type Story = StoryObj<typeof meta>;

export const Default: Story = {
  render: () => (
    <Paper className={"max-w-md p-6"}>
      <p>{"Content grouped on a paper surface."}</p>
    </Paper>
  ),
};
