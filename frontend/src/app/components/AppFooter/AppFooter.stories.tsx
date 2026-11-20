import type { Meta, StoryObj } from "@storybook/react-vite";
import { createMemoryHistory, createRootRoute, createRouter, RouterProvider } from "@tanstack/react-router";
import AppFooter from "./AppFooter";

const route = createRootRoute({ component: AppFooter });
const router = createRouter({ routeTree: route, history: createMemoryHistory({ initialEntries: ["/"] }) });

const meta = {
  title: "App/AppFooter",
  component: AppFooter,
  render: () => <RouterProvider router={router} />,
  parameters: {
    docs: {
      description: {
        component: "Application footer with the site copyright notice.",
      },
    },
  },
} satisfies Meta<typeof AppFooter>;

export default meta;

type Story = StoryObj<typeof meta>;

export const Default: Story = {};
