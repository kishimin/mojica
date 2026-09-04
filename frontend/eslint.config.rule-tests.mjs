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
