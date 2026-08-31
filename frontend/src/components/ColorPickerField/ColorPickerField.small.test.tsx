import { fireEvent, render, screen } from "@testing-library/react";
import { useState } from "react";
import { describe, expect, test, vi } from "vitest";
import ColorPickerField from "./ColorPickerField";
import { setupUser } from "@/tests/test-utils";

const ControlledColorPickerField = () => {
  const [value, setValue] = useState("#FFD400");

  return (
    <ColorPickerField
      label={"Color"}
      colorPickerLabel={"Choose color"}
      value={value}
      onChange={setValue}
    />
  );
};

describe("ColorPickerField", () => {
  test("displays the initial HEX value in both color controls", () => {
    render(
      <ColorPickerField
        label={"Color"}
        colorPickerLabel={"Choose color"}
        value={"#FFD400"}
        onChange={vi.fn<(hex: string) => void>()}
      />,
    );

    expect(screen.getByRole("textbox", { name: "Color" })).toHaveValue(
      "#FFD400",
    );
    expect(screen.getByLabelText("Choose color")).toHaveValue("#ffd400");
  });

  test("synchronizes the color control after editing the HEX textbox", async () => {
    const user = setupUser();
    render(<ControlledColorPickerField />);

    const textbox = screen.getByRole("textbox", { name: "Color" });
    await user.clear(textbox);
    await user.type(textbox, "#00ff00");

    expect(textbox).toHaveValue("#00ff00");
    expect(screen.getByLabelText("Choose color")).toHaveValue("#00ff00");
  });

  test("synchronizes the HEX textbox after choosing a color", () => {
    render(<ControlledColorPickerField />);

    const colorPicker = screen.getByLabelText("Choose color");
    // The browser-native color dialog cannot be driven by userEvent in JSDOM;
    // dispatch the change event to verify the component's synchronization boundary.
    fireEvent.change(colorPicker, { target: { value: "#00ff00" } });

    expect(screen.getByRole("textbox", { name: "Color" })).toHaveValue(
      "#00ff00",
    );
  });

  test("prevents value changes while disabled", () => {
    render(
      <ColorPickerField
        label={"Color"}
        colorPickerLabel={"Choose color"}
        value={"#FFD400"}
        onChange={vi.fn<(hex: string) => void>()}
        disabled={true}
      />,
    );

    const textbox = screen.getByRole("textbox", { name: "Color" });
    const colorPicker = screen.getByLabelText("Choose color");

    expect(textbox).toBeDisabled();
    expect(colorPicker).toBeDisabled();
  });

  test("associates the validation message with the HEX textbox", () => {
    render(
      <ColorPickerField
        label={"Color"}
        colorPickerLabel={"Choose color"}
        value={"not-a-color"}
        onChange={vi.fn<(hex: string) => void>()}
        errorMessage={"Enter a valid HEX color"}
      />,
    );

    expect(screen.getByText("Enter a valid HEX color")).toBeVisible();
    expect(screen.getByRole("group", { name: "Color" })).toHaveClass(
      "border-destructive",
    );
    expect(
      screen.getByRole("textbox", { name: "Color" }),
    ).toHaveAccessibleErrorMessage("Enter a valid HEX color");
  });
});
