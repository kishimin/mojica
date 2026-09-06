import { AlertCircle } from "lucide-react";
import { Alert, AlertDescription, AlertTitle } from "@/components/ui/alert";

type AlertBannerProps = {
  /** Heading announced for the alert. */
  title: string;
  /** Supporting message shown below the heading. */
  description: string;
};

/** Provides the module's public behavior. */
const AlertBanner = ({ title, description }: AlertBannerProps) => (
  <Alert
    variant={"destructive"}
    className={"border-destructive-border bg-destructive-background"}
  >
    <AlertCircle aria-hidden={"true"} />
    <AlertTitle>{title}</AlertTitle>
    <AlertDescription className={"!text-foreground"}>
      {description}
    </AlertDescription>
  </Alert>
);

export default AlertBanner;
