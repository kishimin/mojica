const message =
  "Use the generated image MSW handler for successful POST /images mocks.";

export default {
  meta: {
    type: "suggestion",
    docs: {
      description:
        "Require generated MSW handlers for successful image-generation requests.",
    },
    schema: [],
    messages: { preferGeneratedImageMock: message },
  },
  create(context) {
    return {
      Program(node) {
        const sourceText = context.sourceCode.getText(node);
        if (
          !sourceText.includes('http.post("*/images"') ||
          !sourceText.includes("status: 200")
        ) {
          return;
        }

        context.report({ node, messageId: "preferGeneratedImageMock" });
      },
    };
  },
};
