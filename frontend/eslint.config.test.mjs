import assert from "node:assert/strict";
import { test } from "node:test";
import { fileURLToPath } from "node:url";
import { ESLint } from "eslint";

const eslint = new ESLint({
  overrideConfigFile: fileURLToPath(new URL("./eslint.config.mjs", import.meta.url)),
});

const lintJsx = async (jsx) => {
  const [result] = await eslint.lintText(jsx, {
    filePath: "src/lint-fixture.tsx",
  });

  return result.messages.filter(({ ruleId }) => ruleId === "no-restricted-syntax");
};

test("rejects text written directly inside a JSX tag", async () => {
  const messages = await lintJsx("const Example = () => <p>Use letters only</p>;");

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
