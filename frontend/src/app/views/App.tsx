import { useState } from "react";
import { AppProviders } from "../providers/AppProviders";

/**
 * Placeholder screen until the image generation UI (docs/v1/ui/ui.md) replaces it.
 */
const App = () => {
  const [count, setCount] = useState(0);

  return (
    <AppProviders>
      <button
        type="button"
        className="counter"
        onClick={() => setCount((count) => count + 1)}
      >
        Count is {count}
      </button>
    </AppProviders>
  );
};

export default App;
