import { Component, type ReactNode } from "react";
import ErrorFallback from "@/features/error/views/ErrorFallback";

/* eslint-disable no-restricted-syntax -- React error boundaries require class lifecycle methods. */

type ErrorBoundaryProps = {
  children: ReactNode;
};

type ErrorBoundaryState = {
  hasError: boolean;
};

/** Catches render failures before provider-dependent UI is available. */
class ErrorBoundary extends Component<ErrorBoundaryProps, ErrorBoundaryState> {
  public state: ErrorBoundaryState = { hasError: false };

  /** Switches the boundary to its provider-independent fallback after an error. */
  public static getDerivedStateFromError(): ErrorBoundaryState {
    return { hasError: true };
  }

  /** Renders children until an error requires the recovery screen. */
  public render() {
    return this.state.hasError ? <ErrorFallback /> : this.props.children;
  }
}

export default ErrorBoundary;
