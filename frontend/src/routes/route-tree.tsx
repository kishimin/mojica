import { createRootRoute, createRoute } from "@tanstack/react-router";
import Layout from "@/app/components/Layout/Layout";
import ImageGenerationScreen from "@/features/image-generation/views/ImageGenerationScreen";
import NotFoundView from "@/features/not-found/views/NotFoundView";

const rootRoute = createRootRoute({
  component: Layout,
  notFoundComponent: NotFoundView,
});

const indexRoute = createRoute({
  getParentRoute: () => rootRoute,
  path: "/",
  component: ImageGenerationScreen,
});

export const routeTree = rootRoute.addChildren([indexRoute]);
