import { useState } from "react";
import { AppProviders } from "../providers/AppProviders";

/** Placeholder screen for the application root. */
const App = () => {
  const [count, setCount] = useState(0);

  return (
    <AppProviders>
      <button
        type={"button"}
        className={"counter"}
        onClick={() => setCount((count) => count + 1)}
      >
        {"Count is "}
        {count}
      </button>
    </AppProviders>
  );
};

export default App;
