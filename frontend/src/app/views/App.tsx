import AppFooter from "../components/AppFooter/AppFooter";
import AppHeader from "../components/AppHeader/AppHeader";
import { AppProviders } from "../providers/AppProviders";
import ImageGenerationScreen from "@/features/image-generation/views/ImageGenerationScreen";

const App = () => (
  <AppProviders>
    <div className={"flex min-h-screen flex-col overflow-x-clip"}>
      <AppHeader />
      <ImageGenerationScreen />
      <AppFooter />
    </div>
  </AppProviders>
);

export default App;
