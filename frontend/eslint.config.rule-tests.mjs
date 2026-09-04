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
