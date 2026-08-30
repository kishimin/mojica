import { AlertCircle } from "lucide-react";
import { Alert, AlertDescription, AlertTitle } from "@/components/ui/alert";

type AlertBannerProps = {
  title: string;
  description: string;
};

const AlertBanner = ({ title, description }: AlertBannerProps) => (
  <Alert
    variant={"destructive"}
    className={"border-destructive-border bg-destructive-background"}
  >
    <AlertCircle aria-hidden={"true"} />
    <AlertTitle>{title}</AlertTitle>
    <AlertDescription>{description}</AlertDescription>
  </Alert>
);

export default AlertBanner;
