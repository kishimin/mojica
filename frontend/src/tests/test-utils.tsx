import {
  render as testingLibraryRender,
  type RenderOptions,
} from "@testing-library/react";
import userEvent from "@testing-library/user-event";
import type { ReactElement } from "react";

export const setup = (ui: ReactElement, options?: RenderOptions) => ({
  user: userEvent.setup(),
  ...testingLibraryRender(ui, options),
});
