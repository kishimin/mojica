import type { RouterHistory } from "@tanstack/history";
import { createBrowserHistory, createRouter } from "@tanstack/react-router";
import { routeTree } from "@/routes/route-tree";

type AppRouterOptions = {
  history?: RouterHistory;
};

/** Provides the module's public behavior. */
export const createAppRouter = ({
  history = createBrowserHistory(),
}: AppRouterOptions = {}) => createRouter({ routeTree, history });

export const router = createAppRouter();
