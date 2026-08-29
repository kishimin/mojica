import { afterEach, describe, expect, test } from "vitest";

import "./globals.css";

const rootStyles = () => getComputedStyle(document.documentElement);

const cssVariable = (name: string) =>
  rootStyles().getPropertyValue(name).trim();

describe("global design tokens", () => {
  afterEach(() => {
    document.documentElement.classList.remove("dark");
  });

  test("exposes the approved light-theme colors and radii", () => {
    expect(cssVariable("--background")).toBe("oklch(0.978 0.008 225.1)");
    expect(cssVariable("--surface")).toBe("oklch(1 0 0)");
    expect(cssVariable("--foreground")).toBe("oklch(0.229 0.006 56.1)");
    expect(cssVariable("--primary")).toBe("oklch(0.793 0.088 227.9)");
    expect(cssVariable("--destructive")).toBe("oklch(0.541 0.194 26.7)");
    expect(cssVariable("--radius-sm")).toBe("0.5rem");
    expect(cssVariable("--radius-md")).toBe("0.625rem");
    expect(cssVariable("--radius-lg")).toBe("0.75rem");
    expect(cssVariable("--radius-xl")).toBe("1.125rem");
  });

  test("keeps the approved light palette when a dark class is present", () => {
    const background = cssVariable("--background");
    const foreground = cssVariable("--foreground");

    document.documentElement.classList.add("dark");

    expect(cssVariable("--background")).toBe(background);
    expect(cssVariable("--foreground")).toBe(foreground);
  });

  test("exposes the shared typography and responsive layout foundation", () => {
    expect(cssVariable("--text-xs-medium")).toBe("0.8125rem");
    expect(cssVariable("--text-md")).toBe("1rem");
    expect(cssVariable("--text-7xl")).toBe("4.5rem");
    expect(cssVariable("--layout-content-max-width")).toBe("45rem");
    expect(cssVariable("--layout-header-height")).toBe("5.5rem");
    expect(cssVariable("--layout-header-inline-padding")).toBe("3.5rem");
    expect(cssVariable("--layout-main-padding-top")).toBe("3rem");
  });
});
