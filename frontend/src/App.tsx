import { useState } from "react";

/**
 * Placeholder screen until the image generation UI (docs/v1/ui/ui.md) replaces it.
 */
function App() {
  const [count, setCount] = useState(0);

  return (
    <button
      type="button"
      className="counter"
      onClick={() => setCount((count) => count + 1)}
    >
      Count is {count}
    </button>
  );
}

export default App;
