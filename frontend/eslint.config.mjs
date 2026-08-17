import js from "@eslint/js";
import vitest from "@vitest/eslint-plugin";
import eslintComments from "@eslint-community/eslint-plugin-eslint-comments";
import { defineConfig, globalIgnores } from "eslint/config";
import prettier from "eslint-config-prettier";
import importPlugin from "eslint-plugin-import";
import jsdocPlugin from "eslint-plugin-jsdoc";
import oxlint from "eslint-plugin-oxlint";
import react from "eslint-plugin-react";
import testingLibrary from "eslint-plugin-testing-library";
import unusedImports from "eslint-plugin-unused-imports";
import globals from "globals";
import tseslint from "typescript-eslint";

export default defineConfig([
  // Global ignores
  globalIgnores(["dist", "coverage/**"]),

  // Unused imports (applies broadly)
  {
    plugins: {
      "unused-imports": unusedImports,
      "@eslint-community/eslint-comments": eslintComments,
    },
    rules: {
      "@typescript-eslint/no-unused-vars": "off",
      "unused-imports/no-unused-imports": "error",
      "unused-imports/no-unused-vars": [
        "warn",
        {
          vars: "all",
          varsIgnorePattern: "^_",
          args: "after-used",
          argsIgnorePattern: "^_",
        },
      ],
      "@eslint-community/eslint-comments/require-description": "error",
    },
  },

  // TypeScript + React files
  {
    files: ["**/*.{ts,tsx}"],
    extends: [
      js.configs.recommended,
      tseslint.configs.recommended,
      tseslint.configs.recommendedTypeChecked,
      react.configs.flat.recommended,
      importPlugin.flatConfigs.recommended,
      importPlugin.flatConfigs.typescript,
    ],
    settings: {
      react: {
        version: "detect",
      },
      "import/resolver": {
        typescript: true,
        node: true,
      },
    },
    languageOptions: {
      ecmaVersion: 2020,
      globals: globals.browser,
      parserOptions: { projectService: true },
    },
    rules: {
      "no-console": "warn",
      camelcase: ["warn", { properties: "never" }],
      "@typescript-eslint/switch-exhaustiveness-check": "warn",
      "@typescript-eslint/no-explicit-any": "warn",
      "@typescript-eslint/no-unnecessary-type-assertion": "off",
      "import/order": [
        "error",
        {
          alphabetize: {
            order: "asc",
            caseInsensitive: true,
          },
        },
      ],
      "react/jsx-key": ["error", { checkFragmentShorthand: true }],
      "react/react-in-jsx-scope": 0,
      "react/jsx-uses-react": 0,
    },
  },

  // Test files: Testing Library + Vitest
  {
    files: [
      "**/*.{small,medium,large}.test.{ts,tsx}",
      "**/*.test.{ts,tsx}",
      "**/*.spec.{ts,tsx}",
      "**/__tests__/**/*.{ts,tsx}",
      "**/tests/**/**/*.{ts,tsx}",
    ],
    ...testingLibrary.configs["flat/react"],
    plugins: {
      ...testingLibrary.configs["flat/react"].plugins,
      vitest,
    },
    rules: {
      ...testingLibrary.configs["flat/react"].rules,
      ...vitest.configs.recommended.rules,
      "@typescript-eslint/no-unsafe-call": "off",
      "@typescript-eslint/no-unsafe-member-access": "off",
      "vitest/max-nested-describe": ["error", { max: 3 }],
      "vitest/no-focused-tests": "error",
      "vitest/no-disabled-tests": "warn",
    },
    settings: {
      vitest: { typecheck: true },
    },
    languageOptions: { globals: { ...vitest.environments.env.globals } },
  },

  // JSDoc rules
  jsdocPlugin.configs["flat/recommended"],
  {
    rules: {
      "jsdoc/require-param": "off",
      "jsdoc/require-returns": "off",
      "jsdoc/require-description": "off",
      "jsdoc/check-values": [
        "error",
        {
          allowedLicenses: ["MIT", "ISC"],
        },
      ],
      "jsdoc/require-jsdoc": [
        "error",
        {
          publicOnly: true,
          require: {
            FunctionDeclaration: true,
            MethodDefinition: true,
            ClassDeclaration: true,
          },
        },
      ],
    },
    settings: {
      structuredTags: {
        see: {
          name: "namepath-referencing",
          required: ["name"],
        },
      },
    },
  },

  // Disable type-checked rules for config/tooling files
  {
    files: ["*.config.{js,mjs,ts,mts}", "vitest.setup.ts"],
    extends: [tseslint.configs.disableTypeChecked],
    languageOptions: {
      globals: globals.node,
    },
    rules: {
      "testing-library/prefer-screen-queries": "off",
    },
  },

  // Prettier must be last, before oxlint takes over disabling its own rules
  prettier,

  // Turn off every rule oxlint already covers (see .oxlintrc.json) so the
  // same violation isn't reported twice; run `oxlint` before `eslint`.
  ...oxlint.buildFromOxlintConfigFile("./.oxlintrc.json"),
]);
