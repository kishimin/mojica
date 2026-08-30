import logoImage from "@/assets/logo.svg";

/** Displays the Mojica brand mark and wordmark. */
const Logo = () => (
  <div className={"inline-flex items-center gap-3"}>
    <img src={logoImage} alt={""} className={"size-6"} />
    <span className={"text-2xl font-semibold"}>mojica</span>
  </div>
);

export default Logo;
