import { render, screen } from "@testing-library/react";
import { describe, expect, test } from "vitest";
import AppFooter from "./AppFooter";

describe("AppFooter", () => {
  // ID: APP-FOOTER-S-000
  // Source: docs/v1/ui/components/AppFooter.md § Responsibility, § Tests
  // Given: The application footer is rendered
  // When: Assistive technology inspects the page landmarks
  // Then: The footer is exposed as a contentinfo landmark
  // Blocked by: AppFooter implementation
  // Priority: P0
  test("renders the application footer as a contentinfo landmark", () => {
    render(<AppFooter />);

    expect(screen.getByRole("contentinfo")).toBeVisible();
  });

  // ID: APP-FOOTER-S-001
  // Source: docs/v1/ui/components/AppFooter.md § Storybook, § Tests
  // Given: The application footer is rendered
  // When: The user reads the footer
  // Then: The documented copyright text is displayed
  // Blocked by: AppFooter implementation
  // Priority: P1
  test.todo("displays the documented copyright text");
});
