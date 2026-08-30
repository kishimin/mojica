import { fireEvent, render, screen } from "@testing-library/react";
import userEvent from "@testing-library/user-event";
import { useState } from "react";
import { describe, expect, test, vi } from "vitest";
import ColorPickerField from "./ColorPickerField";

const ControlledColorPickerField = () => {
  const [value, setValue] = useState("#FFD400");

  return <ColorPickerField label={"Color"} value={value} onChange={setValue} />;
};

describe("ColorPickerField", () => {
  test("displays the initial HEX value in both color controls", () => {
    render(
      <ColorPickerField
        label={"Color"}
        value={"#FFD400"}
        onChange={vi.fn<(hex: string) => void>()}
      />,
    );

    expect(screen.getByRole("textbox", { name: "Color" })).toHaveValue(
      "#FFD400",
    );
    expect(screen.getByLabelText("Color picker")).toHaveValue("#ffd400");
  });

  test("synchronizes the color control after editing the HEX textbox", async () => {
    const user = userEvent.setup();
    render(<ControlledColorPickerField />);

    const textbox = screen.getByRole("textbox", { name: "Color" });
    await user.clear(textbox);
    await user.type(textbox, "#00ff00");

    expect(textbox).toHaveValue("#00ff00");
    expect(screen.getByLabelText("Color picker")).toHaveValue("#00ff00");
  });

  test("synchronizes the HEX textbox after choosing a color", () => {
    render(<ControlledColorPickerField />);

    const colorPicker = screen.getByLabelText("Color picker");
    fireEvent.change(colorPicker, { target: { value: "#00ff00" } });

    expect(screen.getByRole("textbox", { name: "Color" })).toHaveValue(
      "#00ff00",
    );
  });

  test("prevents value changes while disabled", async () => {
    const user = userEvent.setup();
    render(
      <ColorPickerField
        label={"Color"}
        value={"#FFD400"}
        onChange={vi.fn<(hex: string) => void>()}
        disabled={true}
      />,
    );

    const textbox = screen.getByRole("textbox", { name: "Color" });
    const colorPicker = screen.getByLabelText("Color picker");

    expect(textbox).toBeDisabled();
    expect(colorPicker).toBeDisabled();

    await user.click(textbox);
    await user.type(textbox, "#00ff00");

    expect(textbox).toHaveValue("#FFD400");
    expect(colorPicker).toHaveValue("#ffd400");
  });

  test("associates the validation message with the HEX textbox", () => {
    render(
      <ColorPickerField
        label={"Color"}
        value={"not-a-color"}
        onChange={vi.fn<(hex: string) => void>()}
        errorMessage={"Enter a valid HEX color"}
      />,
    );

    expect(screen.getByText("Enter a valid HEX color")).toBeVisible();
    expect(
      screen.getByRole("textbox", { name: "Color" }),
    ).toHaveAccessibleErrorMessage("Enter a valid HEX color");
  });
});
