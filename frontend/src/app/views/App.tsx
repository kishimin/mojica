import { RouterProvider } from "@tanstack/react-router";
import { AppProviders } from "../providers/AppProviders";
import { router } from "@/lib/router";

/** Provides the module's public behavior. */
const App = () => (
  <AppProviders>
    <RouterProvider router={router} />
  </AppProviders>
);

export default App;
