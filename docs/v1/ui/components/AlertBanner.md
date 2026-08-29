# AlertBanner

- Layer: Shared UI
- Location: `components/AlertBanner/AlertBanner.tsx`
- Implementation: shadcn/ui `Alert`/`AlertTitle`/`AlertDescription` + Lucide `AlertCircle`
- Responsibility: A `role="alert"` banner used for errors that are not associated with a field

A shared UI component that displays errors not associated with a particular input field in a banner at the top of the screen. It accepts a heading and description and renders them in `AlertTitle` and `AlertDescription`, respectively. It has no knowledge of API status codes, error codes, or translated copy; the caller determines the actual content.

## Props

```typescript
// An application-owned component under components/AlertBanner that composes ui/Alert
type AlertBannerProps = {
  title: string;
  description: string;
};
```

## Storybook

| Main story state                   | Verification |
| ---------------------------------- | ------------ |
| Default (with heading and details) | `getByRole("alert")` and display of the heading and description |

## Tests

- Size: Small
- Verifies: `title` is rendered as `AlertTitle` and `description` as `AlertDescription` within the element with `role="alert"`
