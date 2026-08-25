# ImageGenerationForm

- Layer: Feature UI
- Location: `features/image-generation/components/ImageGenerationForm/ImageGenerationForm.tsx`
- Implementation: Composition of shared UI components
- Responsibility: Coordinates form state, submission, and error display using `useImageGenerationForm` and the Orval-generated mutation hook from `gen/api/`

## Props

```typescript
// features/image-generation/components/ImageGenerationForm
type ImageGenerationFormProps = {
  locale: "ja" | "en";
};
```

## State model

Do not define a custom discriminated union for submission states such as pending, success, and failure. Use the `isPending`, `isError`, `isSuccess`, and `error` values returned directly by the Orval-generated mutation hook (TanStack Query `useMutation`) in `gen/api/`. This follows the frontend-state principle of not duplicating server state in a UI-side type. The generated hook already represents the equivalent of a `SubmissionState`; adding another discriminated union would duplicate server state in the UI.

- For field validation errors, do not define custom `FieldErrors` or `FieldName` types. Use React Hook Form's `formState.errors` (`FieldErrors<ImageGenerationFormValues>`) directly, with `ImageGenerationFormValues` inferred from `imageGenerationSchema` using `z.infer`.
- Define the client validation rules from ui.md §11 as the Zod schema in `imageGenerationSchema.ts`, and connect it through `useForm({ resolver: zodResolver(imageGenerationSchema) })`. `handleSubmit` calls `onSubmit` only after validation succeeds; do not reimplement submission blocking.
- From `handleSubmit`'s `onSubmit`, call the Orval-generated mutation hook through `mutate` or `mutateAsync`. Handle `422 Unprocessable Entity` in the hook's `onError` callback and map each `errors[].field` back to the same React Hook Form field with `setError(field, { type: "server", message })`. The API result is authoritative even when the input passed frontend validation (ui.md §11, API-side validation errors).
- For `400`, `429`, `500`, `502`, and `504`, read the API's language-independent `code` from the generated hook's `isError`/`error` state. Map it to the heading from ui.md §12 with `toImageGenerationErrorPresentation`. Pass that heading and the API response's localized `message` directly to [AlertBanner](./AlertBanner.md), without exposing internal error details. Do not branch display behavior on HTTP status codes.
- When a `429` response includes a `Retry-After` header, pass its seconds to `useRetryAfterCountdown`. While at least one second remains, pass `{ kind: "cooldown", remainingSeconds }` to [GenerateButton](./GenerateButton.md); once it reaches zero, pass `{ kind: "retryable" }` (ui.md §12, “Retry-After for 429”). For an API error without the header, make it retryable immediately.
- On success, use the `onSuccess` callback to download the response PNG automatically with the filename from `Content-Disposition` (ui.md §10 specifies that no preview is retained).
- While submitting, derive `{ kind: "submitting" }` for [GenerateButton](./GenerateButton.md) from the generated hook's `isPending` and `formState.isSubmitting`, preventing duplicate requests (ui.md §9). Decide the precedence of submitting, cooldown, retryable, and idle once in `ImageGenerationForm`; do not duplicate state transitions inside the button.

## Validation schema (Zod)

```typescript
// features/image-generation/schemas/imageGenerationSchema.ts
import { z } from "zod";

export const imageGenerationSchema = z
  .object({
    text: z.string().trim().min(1).max(64),
    foregroundCharacter: z.string().min(1).max(128),
    foregroundColor: z.string(),
    backgroundCharacter: z.string().min(1).max(128),
    backgroundColor: z.string(),
    type: z.enum(["standard", "x-background", "x-icon"]),
  })
  .refine(
    (values) =>
      values.foregroundCharacter.trim() !== "" ||
      values.backgroundCharacter.trim() !== "",
    {
      message: "foregroundOrBackgroundRequired",
      path: ["foregroundCharacter"],
    },
  );

export type ImageGenerationFormValues = z.infer<typeof imageGenerationSchema>;
```

Map every rule in ui.md §11—length, required values, control-character rejection, whitespace-only rejection, and so on—directly to the Zod method chain. Express regular-expression rules such as rejecting control characters with `.regex()`. Store only message keys such as `foregroundOrBackgroundRequired`; resolve displayed copy through the i18n translation function described in ui.md §13.

`useImageGenerationForm.ts` handles only React Hook Form input state, the `imageGenerationSchema` resolver wiring, and `defaultValues`. Rule coverage belongs exclusively to `imageGenerationSchema` and must not be duplicated.

## Asynchronous boundary

This screen performs no initial data fetch. Its only asynchronous operation is the `POST /images` mutation from `gen/api/`, started by pressing the button. Because it does not use `useQuery`, do not use `<Suspense>`. Represent pending, success, and failure through the mutation hook's `isPending`, `isError`, and `isSuccess` states, without a page-wide loading view.

As the final defense against unexpected render failures, place one `<ErrorBoundary>` at the application root in `app/providers/AppProviders.tsx`. Failures from `POST /images` are represented through the mutation hook's `isError`/`error`, so do not add a separate boundary under `ImageGenerationForm`. [ErrorFallback](./ErrorFallback.md) supplies the fallback.

## Storybook

| Main story state | Verification |
| ---------------- | ------------ |
| Default (empty) / Filled / ValidationError (from Zod schema) / ServerError (`setError` from 422) / Submitting | Client validation, field errors from a `422`, and prevention of duplicate clicks while submitting (`play`) |

Mock `POST /images` with MSW's `http.post`; do not connect to the real API.

## Tests

| Size  | Subject | Verification |
| ----- | ------- | ------------ |
| Small | `imageGenerationSchema.ts` | Every rule in ui.md §11: required values, length limits, whitespace-only rejection, control-character rejection, and the character combination |
| Small | `useImageGenerationForm.ts` | Resolver wiring and `defaultValues` |
| Small | `useRetryAfterCountdown.ts` | Initial seconds, decrement once per second, stopping at zero, restarting when input changes, and timer disposal on unmount, using fake timers |
| Small | `toImageGenerationErrorPresentation.ts` | Mapping every API `code` to the heading in ui.md §12 and classifying unsupported codes as fallback |
| Small | `ImageGenerationForm.tsx` | The feature's central integration test: MSW mocks `POST /images` in the same process, intercepting Axios inside the generated Orval mutation hook before the real network; use `userEvent` to verify input → submission → success / 422 (`setError` from `errors[].field`) / 400, 429, 500, 502, and 504 (`AlertBanner`) |

Do not create dedicated tests for the Orval-generated mutation-hook implementation in `gen/api/`. Generated code must not be edited manually, and Orval owns its generation logic. `ImageGenerationForm.small.test.tsx` verifies its types, request, and response handling through the actual usage path.
