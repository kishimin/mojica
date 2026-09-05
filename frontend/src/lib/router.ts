import type { RouterHistory } from "@tanstack/history";
import { createBrowserHistory, createRouter } from "@tanstack/react-router";
import { routeTree } from "@/routes/route-tree";

type AppRouterOptions = {
  history?: RouterHistory;
};

export const createAppRouter = ({
  history = createBrowserHistory(),
}: AppRouterOptions = {}) => createRouter({ routeTree, history });

export const router = createAppRouter();
