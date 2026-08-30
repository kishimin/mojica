import { render, screen } from "@testing-library/react";
import userEvent from "@testing-library/user-event";
import { describe, expect, test } from "vitest";
import TextField from "./TextField";

describe("TextField", () => {
  test("accepts text through the labeled textbox", async () => {
    const user = userEvent.setup();
    render(<TextField label={"Text"} />);

    const textbox = screen.getByRole("textbox", { name: "Text" });
    await user.type(textbox, "Hello");

    expect(textbox).toHaveValue("Hello");
  });

  test("associates the validation message with the textbox", () => {
    render(<TextField label={"Text"} errorMessage={"Text is required"} />);

    expect(screen.getByText("Text is required")).toBeVisible();
    expect(
      screen.getByRole("textbox", { name: "Text" }),
    ).toHaveAccessibleErrorMessage("Text is required");
  });

  test("displays the placeholder supplied by the caller", () => {
    render(<TextField label={"Text"} placeholder={"Enter text"} />);

    expect(screen.getByRole("textbox", { name: "Text" })).toHaveAttribute(
      "placeholder",
      "Enter text",
    );
  });

  test("retains the caller description when adding a validation error", () => {
    render(
      <>
        <p id={"text-hint"}>{"Use letters only"}</p>
        <TextField
          label={"Text"}
          aria-describedby={"text-hint"}
          errorMessage={"Text is required"}
        />
      </>,
    );

    expect(
      screen.getByRole("textbox", { name: "Text" }),
    ).toHaveAccessibleDescription("Use letters only");
  });
});
