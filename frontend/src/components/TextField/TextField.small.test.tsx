import { render, screen } from "@testing-library/react";
import userEvent from "@testing-library/user-event";
import { describe, expect, test } from "vitest";

import TextField from "./TextField";

describe("TextField", () => {
  test("renders a labeled textbox and accepts user input", async () => {
    const user = userEvent.setup();

    render(<TextField label={"Prompt"} name={"prompt"} />);

    const textbox = screen.getByRole("textbox", { name: "Prompt" });

    await user.type(textbox, "A sunset over the sea");

    expect(textbox).toHaveValue("A sunset over the sea");
    expect(textbox).toHaveAttribute("name", "prompt");
  });

  test("associates the validation message with the textbox", () => {
    render(
      <TextField
        label={"Prompt"}
        errorMessage={"Prompt is required"}
      />,
    );

    const textbox = screen.getByRole("textbox", { name: "Prompt" });
    const error = screen.getByText("Prompt is required");

    expect(textbox).toHaveAccessibleDescription("Prompt is required");
    expect(textbox).toHaveAttribute("aria-describedby", error.id);
  });
});
