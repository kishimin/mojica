const isLocaleParameter = (parameter) =>
  parameter.type === "Identifier" && parameter.name === "locale";

const hasLocaleParameter = (node) =>
  node.params.some((parameter) => {
    if (parameter.type === "AssignmentPattern") {
      return isLocaleParameter(parameter.left);
    }

    return isLocaleParameter(parameter);
  });

export default {
  meta: {
    type: "problem",
    docs: {
      description: "Require locale-dependent E2E selectors to use maps",
    },
    schema: [],
    messages: {
      selector:
        "Define locale-dependent E2E selectors as a locale map instead of a function.",
    },
  },
  create(context) {
    return {
      ArrowFunctionExpression(node) {
        if (hasLocaleParameter(node)) {
          context.report({ node, messageId: "selector" });
        }
      },
      FunctionExpression(node) {
        if (hasLocaleParameter(node)) {
          context.report({ node, messageId: "selector" });
        }
      },
    };
  },
};
