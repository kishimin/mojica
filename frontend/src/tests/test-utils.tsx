import { render as testingLibraryRender } from "@testing-library/react";
import userEvent from "@testing-library/user-event";
import type { ReactElement } from "react";

export const setup = (
  element: ReactElement,
  options?: Parameters<typeof testingLibraryRender>[1],
) => ({
  user: userEvent.setup(),
  ...testingLibraryRender(element, options),
});
