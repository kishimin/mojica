import { createRootRoute, createRoute } from "@tanstack/react-router";
import Layout from "@/app/components/Layout/Layout";
import ImageGenerationScreen from "@/features/image-generation/views/ImageGenerationScreen";

const rootRoute = createRootRoute({ component: Layout });

const indexRoute = createRoute({
  getParentRoute: () => rootRoute,
  path: "/",
  component: ImageGenerationScreen,
});

export const routeTree = rootRoute.addChildren([indexRoute]);
