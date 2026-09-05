import assert from "node:assert/strict";
import { test } from "node:test";
import { fileURLToPath } from "node:url";
import { ESLint } from "eslint";

const eslint = new ESLint({
  overrideConfigFile: fileURLToPath(
    new URL("./eslint.config.mjs", import.meta.url),
  ),
});

const lintJsx = async (jsx) => {
  const [result] = await eslint.lintText(jsx, {
    filePath: "src/components/Logo/Logo.tsx",
  });

  return result.messages.filter(
    ({ ruleId }) => ruleId === "no-restricted-syntax",
  );
};

const lintTypeScript = async (source) => {
  const [result] = await eslint.lintText(source, {
    filePath: "src/types/image-type.ts",
  });

  return result.messages.filter(
    ({ ruleId }) => ruleId === "local/prefer-object-derived-union",
  );
};

const lintExplicitAny = async (source) => {
  const [result] = await eslint.lintText(source, {
    filePath: "src/types/image-type.ts",
  });

  return result.messages.filter(
    ({ ruleId }) => ruleId === "@typescript-eslint/no-explicit-any",
  );
};

const lintImageMocks = async (source) => {
  const [result] = await eslint.lintText(source, {
    filePath:
      "src/features/image-generation/components/ImageGenerationForm/ImageGenerationForm.small.test.tsx",
  });

  return result.messages.filter(
    ({ ruleId }) => ruleId === "local/prefer-generated-image-msw-handler",
  );
};

const lintUtils = async (
  source,
  filePath = "src/features/image-generation/utils/toImageGenerationErrorPresentation.ts",
) => {
  const [result] = await eslint.lintText(source, { filePath });

  return result.messages.filter(
    ({ ruleId }) => ruleId === "local/prefer-named-exports-in-utils",
  );
};

const lintProps = async (source) => {
  const [result] = await eslint.lintText(source, {
    filePath: "src/components/AlertBanner/AlertBanner.tsx",
  });

  return result.messages.filter(
    ({ ruleId }) => ruleId === "local/limit-props-keys",
  );
};

const lintFunctionParameters = async (source) => {
  const [result] = await eslint.lintText(source, {
    filePath: "src/components/Logo/Logo.tsx",
  });

  return result.messages.filter(({ ruleId }) => ruleId === "max-params");
};

const lintFunctionLength = async (source) => {
  const [result] = await eslint.lintText(source, {
    filePath:
      "src/features/image-generation/utils/toImageGenerationErrorPresentation.ts",
  });

  return result.messages.filter(
    ({ ruleId }) => ruleId === "max-lines-per-function",
  );
};

const lintFormFields = async (source) => {
  const [result] = await eslint.lintText(source, {
    filePath:
      "src/features/image-generation/components/ImageGenerationForm/ImageGenerationForm.tsx",
  });

  return result.messages.filter(
    ({ ruleId }) => ruleId === "local/require-blank-line-between-form-fields",
  );
};

const lintE2e = async (source, filePath) => {
  const [result] = await eslint.lintText(source, { filePath });

  return result.messages.filter(({ ruleId }) => ruleId?.startsWith("local/"));
};

const lintE2ePageObject = async (source) => {
  const [result] = await eslint.lintText(source, {
    filePath: "e2e/pages/example-page.ts",
  });

  return result.messages.filter(
    ({ ruleId }) =>
      ruleId === "local/require-e2e-page-object-method-references",
  );
};

test("rejects text written directly inside a JSX tag", async () => {
  const messages = await lintJsx(
    "const Example = () => <p>Use letters only</p>;",
  );

  assert.deepEqual(
    messages.map(({ message }) => message),
    ["Wrap JSX text in braces."],
  );
});

test("allows JSX text written as a string expression", async () => {
  const messages = await lintJsx(
    'const Example = () => <p>{"Use letters only"}</p>;',
  );

  assert.deepEqual(messages, []);
});

test("allows whitespace used to format nested JSX", async () => {
  const messages = await lintJsx(
    "const Example = () => <div>\n  <span />\n</div>;",
  );

  assert.deepEqual(messages, []);
});

test("rejects let declarations", async () => {
  const messages = await lintJsx("let value = 1;");

  assert.deepEqual(
    messages.map(({ message }) => message),
    ["Use const instead of let."],
  );
});

test("rejects explicit any assertions", async () => {
  const messages = await lintExplicitAny("const value = input as any;");

  assert.deepEqual(
    messages.map(({ ruleId }) => ruleId),
    ["@typescript-eslint/no-explicit-any"],
  );
});

test("rejects inline string literal unions", async () => {
  const messages = await lintTypeScript(
    'type ImageType = "standard" | "x-background" | "x-icon";',
  );

  assert.deepEqual(
    messages.map(({ ruleId, message }) => ({ ruleId, message })),
    [
      {
        ruleId: "local/prefer-object-derived-union",
        message:
          "Define string values in an `as const` object and derive the union type from its values.",
      },
    ],
  );
});

test("allows object-derived string unions", async () => {
  const messages = await lintTypeScript(`
    const imageTypeDefinitions = {
      standard: "standard",
      xBackground: "x-background",
      xIcon: "x-icon",
    } as const;
    type ImageType =
      (typeof imageTypeDefinitions)[keyof typeof imageTypeDefinitions];
  `);

  assert.deepEqual(messages, []);
});

test("rejects successful inline image MSW handlers", async () => {
  const messages = await lintImageMocks(`
    http.post("*/images", () => new HttpResponse(null, { status: 200 }));
  `);

  assert.deepEqual(
    messages.map(({ ruleId, message }) => ({ ruleId, message })),
    [
      {
        ruleId: "local/prefer-generated-image-msw-handler",
        message:
          "Use the generated image MSW handler for successful POST /images mocks.",
      },
    ],
  );
});

test("allows error responses to use an inline image MSW handler", async () => {
  const messages = await lintImageMocks(`
    http.post("*/images", () => HttpResponse.json({}, { status: 422 }));
  `);

  assert.deepEqual(messages, []);
});

test("rejects default exports from utility files", async () => {
  const messages = await lintUtils("export default mapValue;");

  assert.deepEqual(
    messages.map(({ ruleId, message }) => ({ ruleId, message })),
    [
      {
        ruleId: "local/prefer-named-exports-in-utils",
        message: "Use named exports in utility files.",
      },
    ],
  );
});

test("allows named exports from utility files", async () => {
  const messages = await lintUtils("export const mapValue = () => null;");

  assert.deepEqual(messages, []);
});

test("allows default exports from component files", async () => {
  const messages = await lintUtils(
    "export default Component;",
    "src/components/Logo/Logo.tsx",
  );

  assert.deepEqual(messages, []);
});

test("rejects functions with more than five parameters", async () => {
  const messages = await lintFunctionParameters(
    "const render = (one, two, three, four, five, six) => null;",
  );

  assert.deepEqual(
    messages.map(({ ruleId }) => ruleId),
    ["max-params"],
  );
});

test("rejects Props definitions with more than five keys", async () => {
  const messages = await lintProps(`
    type ExampleProps = {
      one: string;
      two: string;
      three: string;
      four: string;
      five: string;
      six: string;
    };
  `);

  assert.deepEqual(
    messages.map(({ ruleId, message }) => ({ ruleId, message })),
    [
      {
        ruleId: "local/limit-props-keys",
        message: "Keep Props definitions to five keys or fewer.",
      },
    ],
  );
});

test("allows Props definitions with five keys", async () => {
  const messages = await lintProps(`
    interface ExampleProps {
      one: string;
      two: string;
      three: string;
      four: string;
      five: string;
    }
  `);

  assert.deepEqual(messages, []);
});

test("rejects functions longer than thirty lines", async () => {
  const statements = Array.from(
    { length: 31 },
    (_, index) => `  const value${index} = ${index};`,
  ).join("\n");
  const messages = await lintFunctionLength(
    `const render = () => {\n${statements}\n};`,
  );

  assert.deepEqual(
    messages.map(({ ruleId }) => ruleId),
    ["max-lines-per-function"],
  );
});

test("allows functions with thirty lines", async () => {
  const statements = Array.from(
    { length: 28 },
    (_, index) => `  const value${index} = ${index};`,
  ).join("\n");
  const messages = await lintFunctionLength(
    `const render = () => {\n${statements}\n};`,
  );

  assert.deepEqual(messages, []);
});

test("rejects adjacent form fields without a blank line", async () => {
  const messages = await lintFormFields(
    "const Form = () => <form><TextField /><TextField /></form>;",
  );

  assert.deepEqual(
    messages.map(({ ruleId, message }) => ({ ruleId, message })),
    [
      {
        ruleId: "local/require-blank-line-between-form-fields",
        message: "Separate form fields with a blank line.",
      },
    ],
  );
});

test("allows form fields separated by a blank line", async () => {
  const messages = await lintFormFields(`
    const Form = () => (
      <form>
        <TextField />

        <TextField />
      </form>
    );
  `);

  assert.deepEqual(messages, []);
});

test("rejects Playwright tests outside the dedicated E2E directories", async () => {
  const messages = await lintE2e(
    'import { test } from "../fixtures"; test.skip("planned");',
    "e2e/image-generation.small.test.ts",
  );

  assert.deepEqual(
    messages.map(({ ruleId, message }) => ({ ruleId, message })),
    [
      {
        ruleId: "local/require-e2e-test-directory",
        message: "Place Playwright test files under e2e/tests or e2e/specs.",
      },
    ],
  );
});

test("allows Playwright tests in the dedicated E2E directories", async () => {
  const messages = await lintE2e(
    'import { test } from "../fixtures"; test.skip("planned");',
    "e2e/tests/image-generation.small.test.ts",
  );

  assert.deepEqual(messages, []);
});

test("rejects direct Playwright imports in E2E tests", async () => {
  const messages = await lintE2e(
    'import { test } from "@playwright/test"; test.skip("planned");',
    "e2e/tests/image-generation.small.test.ts",
  );

  assert.deepEqual(
    messages.map(({ ruleId }) => ruleId),
    ["local/require-e2e-fixture-import"],
  );
});

test("rejects raw page operations in E2E tests", async () => {
  const messages = await lintE2e(
    'import { test } from "../fixtures.js"; test("works", async ({ imageGenerationPage, page }) => { await page.getByRole("button").click(); });',
    "e2e/tests/image-generation.small.test.ts",
  );

  assert.deepEqual(
    messages.map(({ ruleId }) => ruleId),
    ["local/no-raw-page-operations-in-e2e"],
  );
});

test("allows E2E tests to use Page Object fixtures", async () => {
  const messages = await lintE2e(
    'import { test } from "../fixtures"; test("works", async ({ imageGenerationPage }) => { await imageGenerationPage.generate(); });',
    "e2e/tests/image-generation.small.test.ts",
  );

  assert.deepEqual(messages, []);
});

test("rejects implemented E2E tests without a Page Object fixture", async () => {
  const messages = await lintE2e(
    'import { test } from "../fixtures.js"; test("works", async ({ page }) => {});',
    "e2e/tests/image-generation.small.test.ts",
  );

  assert.deepEqual(
    messages.map(({ ruleId }) => ruleId),
    ["local/require-e2e-page-fixture"],
  );
});

test("allows assertion-free skipped E2E plans without a Page Object fixture", async () => {
  const messages = await lintE2e(
    'import { test } from "../fixtures.js"; test.skip("planned", async () => {});',
    "e2e/tests/image-generation.small.test.ts",
  );

  assert.deepEqual(messages, []);
});

test("rejects inline methods in Page Object return values", async () => {
  const messages = await lintE2ePageObject(`
    const examplePage = (page) => {
      return { submit: async () => page.getByRole("button").click() };
    };
  `);

  assert.deepEqual(
    messages.map(({ ruleId, message }) => ({ ruleId, message })),
    [
      {
        ruleId: "local/require-e2e-page-object-method-references",
        message:
          "Define Page Object methods before the returned object and return the function reference.",
      },
    ],
  );
});

test("allows function references in Page Object return values", async () => {
  const messages = await lintE2ePageObject(`
    const examplePage = (page) => {
      const submit = async () => page.getByRole("button").click();
      return { submit };
    };
  `);

  assert.deepEqual(messages, []);
});
