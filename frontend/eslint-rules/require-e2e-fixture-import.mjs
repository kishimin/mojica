const testFilePattern = /(?:\.test|\.spec)\.[cm]?[jt]sx?$/;
const e2eDirectoryPattern = /(?:^|\/)e2e\//;

export default {
  meta: {
    type: "problem",
    docs: {
      description: "Import Playwright tests through the project fixture",
    },
    schema: [],
    messages: {
      directImport:
        "Import Playwright test APIs from the project E2E fixture instead of @playwright/test directly.",
    },
  },
  create(context) {
    let isE2eTest = false;

    return {
      Program(_node) {
        const filePath = context.getFilename().replaceAll("\\", "/");
        isE2eTest =
          e2eDirectoryPattern.test(filePath) && testFilePattern.test(filePath);
      },
      ImportDeclaration(node) {
        if (isE2eTest && node.source.value === "@playwright/test") {
          context.report({ node, messageId: "directImport" });
        }
      },
    };
  },
};
