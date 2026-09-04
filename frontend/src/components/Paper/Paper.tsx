import type { ComponentPropsWithoutRef } from "react";
import { cn } from "@/lib/utils";

type PaperProps = ComponentPropsWithoutRef<"div">;

/** Provides a raised surface for grouping related content. */
const Paper = ({ className, ...props }: PaperProps) => (
  <div
    className={cn(
      "rounded-xl bg-surface shadow-[var(--shadow-card)]",
      className,
    )}
    {...props}
  />
);

export default Paper;
