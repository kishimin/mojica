import type { Meta, StoryObj } from "@storybook/react-vite";
import { Alert, AlertDescription, AlertTitle } from "./alert";

const meta = {
  title: "UI/Alert",
  component: Alert,
  subcomponents: { AlertTitle, AlertDescription },
  args: {
    children: (
      <>
        <AlertTitle>Saved</AlertTitle>
        <AlertDescription>Your changes are now available.</AlertDescription>
      </>
    ),
  },
} satisfies Meta<typeof Alert>;

export default meta;

type Story = StoryObj<typeof meta>;

export const Default: Story = {};

export const Destructive: Story = {
  args: {
    variant: "destructive",
    children: (
      <>
        <AlertTitle>Unable to save</AlertTitle>
        <AlertDescription>Try again after checking your input.</AlertDescription>
      </>
    ),
  },
};
