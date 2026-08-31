# GenerateButton

- Layer: Feature UI
- Location: `features/image-generation/components/GenerateButton/GenerateButton.tsx`
- Implementation: shadcn/ui `Button` + Lucide `Loader2` (`animate-spin`)
- Responsibility: Displays copy, icon, `aria-busy`, and `disabled` according to an exclusive state received from its parent

## Props

```typescript
// features/image-generation/components/GenerateButton
type GenerateButtonProps = {
  state:
    | { kind: "idle" }
    | { kind: "submitting" }
    | { kind: "retryable" }
    | { kind: "cooldown"; remainingSeconds: number };
};
// Internally, when state.kind is submitting, render Lucide's
// <Loader2 className="animate-spin" aria-hidden="true" /> as a child of the
// shared Button and change the text to "生成中..." (Generating...).
```

The `Button` itself does not receive a prop such as `isLoading`. Loading is expressed by composing `Loader2` as a child. Keeping the primitive unchanged avoids polluting the `Button` API with a feature-specific concern. Communicate the state through both `aria-busy` and the displayed copy (`生成中...`, “Generating...”) as specified in ui.md §14.

`state` is a discriminated union of mutually exclusive states. `submitting` and `cooldown` are disabled, while `idle` and `retryable` are enabled. The component does not accept a separate `disabled` value from its caller.

## Display by state

| `kind`       | Display | `disabled` | `aria-busy` |
| ------------ | ------- | ---------- | ----------- |
| `idle`       | `画像を生成する` (Generate image) | false | false |
| `submitting` | Loader2 + `生成中...` (Generating...) | true | true |
| `retryable`  | `画像を生成する` (Generate image) | false | false |
| `cooldown`   | `{remainingSeconds}秒後に再試行できます` (You can retry in ... seconds) | true | false |

`GenerateButton` neither interprets the `Retry-After` header nor owns a timer. `useRetryAfterCountdown` owns the countdown, and [ImageGenerationForm](./ImageGenerationForm.md) maps API errors to `state`.

## Storybook

| Main story state | Verification |
| ---------------- | ------------ |
| Idle / Submitting (shows Lucide `Loader2`) / Retryable / Cooldown | Copy, `aria-busy`, and `disabled` for each `state.kind` |

## Tests

- Size: Small
- Verifies: Displayed copy, Loader2, `aria-busy`, and `disabled` for each `state.kind`. Time passage and timer implementation are not tested here.
