This document integrates the complete latest specification. The header displays the logo image and the “mojica” wordmark, and the footer displays `© kishimin 2026`.

# mojica MVP UI Design Specification

## 1. Overview

The mojica MVP provides a web UI for generating text art images.

Users can enter the required information, select an image type, and generate an image on a single screen.

The generated image is automatically downloaded without displaying a preview.

The UI is responsive and supports desktop, tablet, and smartphone devices.

---

# 2. Screen List

The MVP provides an image generation screen and a 404 Not Found screen.

| Screen                  | Description                                                        |
| ----------------------- | ------------------------------------------------------------------ |
| Image Generation Screen | Generates an image based on specified text, colors, and image type |
| 404 Not Found Screen     | Appears when the user accesses a path that does not exist          |

---

# 3. Header

A header is displayed at the top of the screen.

The mojica logo image and “mojica” wordmark text are placed next to one another on the left side of the header (see §15, “Accessibility”).

The language switcher is placed on the right side of the header. It is a dropdown that displays the selected language name and an expand icon.

Supported languages:

- Japanese
- English

The UI language and the `Accept-Language` header sent to the mojica API are switched according to the selected language.

Japanese:

```http
Accept-Language: ja
```

English:

```http
Accept-Language: en
```

---

# 4. Heading and Description

Place the service heading and description below the header and above the input form.

Heading:

```text
文字で、文字を描く。
```

Description:

```text
好きな文字と2つの色を組み合わせて、文字アート画像を生成します。
```

English localized copy:

```text
Draw letters with letters.
```

```text
Combine your favorite characters and two colors to generate text art.
```

---

# 5. Input Fields

## Text to Render

The user enters the text to be rendered as text art.

Example:

```text
KA
```

Input type:

`text`

Constraints:

- Required
- At least 1 character
- Maximum 64 characters
- Must not consist only of whitespace characters
- Must not contain control characters

---

## Character Used to Render Text

The user enters the character or characters used to construct the rendered text.

Example:

```text
🌻
```

Input type:

`text`

Constraints:

- Required
- At least 1 character
- Maximum 128 characters
- Must not contain control characters
- Whitespace-only input is allowed

---

## Foreground Character Color

Specifies the color of the characters used to render the text.

A color picker is used.

The frontend stores the value in HEX format.

Example:

```text
#FFD400
```

The value is sent to the mojica API in HEX format.

---

## Background Character

The user enters the character or characters used to fill the area surrounding the rendered text.

Example:

```text
☀
```

Input type:

`text`

Constraints:

- Required
- At least 1 character
- Maximum 128 characters
- Must not contain control characters
- Whitespace-only input is allowed

However, both the character used to render the text and the background character must not consist only of whitespace characters at the same time.

---

## Background Character Color

Specifies the color of the characters used to fill the surrounding area.

A color picker is used.

The frontend stores the value in HEX format.

Example:

```text
#FF69B4
```

The value is sent to the mojica API in HEX format.

---

# 6. Image Type

The image type is selected using a select box.

The following options are available:

| Display            | API Value      |
| ------------------ | -------------- |
| Standard Image     | `standard`     |
| X Background Image | `x-background` |
| X Icon Image       | `x-icon`       |

The default value is `standard`.

---

# 7. Generate Image Button

A large image generation button is placed in the center below the input form.

Button label:

```text
Generate Image
```

When the button is pressed, `POST /images` is called.

If the preceding generation attempt failed with an API error, use the Retryable variant defined in §12, “API Error Display.”

---

# 8. Image Generation Flow

The user generates an image through the following steps:

1. Enter the text to render.
2. Enter the character used to render the text.
3. Select the foreground character color.
4. Enter the background character.
5. Select the background character color.
6. Select the image type.
7. Press "Generate Image."
8. Send `POST /images`.
9. Receive the PNG image.
10. Automatically download the image.

The MVP does not provide an image preview after generation.

---

# 9. Generating State

While a request is being sent to the image generation API, the Generate Image button is disabled.

The button label changes to:

```text
Generating...
```

This prevents multiple image generation requests from being submitted while generation is in progress.

The button is enabled again after the request completes or an error occurs.

---

# 10. Download

When image generation succeeds, the received PNG image is automatically downloaded.

No additional user action is required.

The MVP does not provide:

- Image preview
- Manual download button
- Generation history
- Server-side image storage

---

# 11. Validation

The frontend performs validation before sending a request to the API.

## Text to Render

An error occurs in the following cases:

- The field is empty
- The value exceeds 64 characters
- The value consists only of whitespace characters
- The value contains control characters

Japanese message examples:

```text
描画する文字列を入力してください。
```

```text
空白以外の文字を入力してください。
```

```text
描画する文字列は64文字以内で入力してください。
```

```text
描画する文字列には表示可能な文字を含めてください。
```

---

## Character Used to Render Text

An error occurs in the following cases:

- The field is empty
- The value exceeds 128 characters
- The value contains control characters

Japanese message examples:

```text
描画に使う文字を入力してください。
```

```text
描画に使う文字は128文字以内で入力してください。
```

---

## Background Character

An error occurs in the following cases:

- The field is empty
- The value exceeds 128 characters
- The value contains control characters

Japanese message examples:

```text
敷き詰める文字を入力してください。
```

```text
敷き詰める文字は128文字以内で入力してください。
```

---

## Character Combination

An error occurs if both the character used to render the text and the background character consist only of whitespace characters.

Japanese message example:

```text
描画に使う文字または敷き詰める文字のどちらかに、表示可能な文字を入力してください。
```

---

## API Validation Errors

If the API returns `422 Unprocessable Entity`, the frontend uses `errors[].field` to display the error near the corresponding input field.

Even if frontend validation succeeds, API-side validation is treated as the final source of truth.

---

# 12. API Error Display

Errors that are not associated with a specific field are displayed at the top of the input-form card, above every input field.

| Status | Display                        |
| ------ | ------------------------------ |
| 400    | Request error                  |
| 429    | Request limit exceeded         |
| 500    | Server error                   |
| 502    | Image generation service error |
| 504    | Timeout                        |

The localized `message` returned by the API is displayed to the user.

Internal error information must not be displayed.

After an API error, change the generate button to the Retryable color variant to encourage another attempt. Its copy and behavior—calling `POST /images` again—remain the same as normal.

## `Retry-After` for 429

If the API response includes a `Retry-After` header containing the number of seconds before another attempt is allowed, disable the generate button for that duration and show a countdown updated once per second.

```text
{秒数}秒後に再試行できます
```

At zero, enable the button and restore the normal `画像を生成する` (Generate image) label with the Retryable variant. Do not change the banner's localized API `message`. The countdown uses a client-side timer and makes no additional API requests.

If `Retry-After` is absent, do not disable the button; retry is available immediately.

English localized example:

```text
You can try again in {seconds} seconds
```

---

# 13. Internationalization (i18n)

The UI supports switching between Japanese and English.

The language switcher is placed on the right side of the header.

The following UI elements are localized:

- Input field labels
- Buttons
- Select box option labels
- Frontend validation messages
- API error messages
- Status messages such as the generating state

When Japanese is selected:

```http
Accept-Language: ja
```

When English is selected:

```http
Accept-Language: en
```

The corresponding value is sent to the mojica API.

---

# 14. Responsive Design

The UI supports desktop, tablet, and smartphone devices.

The form is primarily arranged in a single-column layout.

On desktop, a maximum width is applied to the form and the form is centered.

On smartphones, the form expands to fit the available screen width.

Horizontal scrolling should generally not occur.

The logo image and language switcher in the header must remain appropriately displayed and operable on smaller screens.

---

# 15. Accessibility

Each form input has an associated label.

The following operations must be possible using only the keyboard:

- Text input
- Color selection
- Image type selection
- Language switching
- Image generation

Error states must not rely on color alone and must also be communicated through text.

While an image is being generated, the button label and state are changed to communicate that processing is in progress.

When an input error occurs, the corresponding field and error message must be programmatically associated for accessibility.

The logo consists of its image and the “mojica” wordmark. Assign `alt=""` to the image to prevent duplicate announcement; the visible “mojica” text supplies the accessible name.

---

# 16. Footer

A footer is displayed at the bottom of the screen.

The following text is displayed:

```text
© kishimin 2026
```

---

# 17. Out of Scope for the MVP

The following features are not implemented in the MVP:

- Image preview
- Custom image size specification
- Generation history
- Login
- User data storage
- Direct sharing to social media
- Server-side storage of generated images
- Filling characters based on an image

---

# 18. MVP UI Goal

The UI enables users to generate and obtain a PNG text art image through the minimum required sequence of operations:

"Enter text → Select colors → Select an image type → Generate the image"

---

# 19. 404 Not Found Screen

Display this screen for a path that does not exist. It shares the same header and footer as the image generation screen.

Heading:

```text
404
```

```text
ページが見つかりません
```

Description:

```text
URLが正しいか確認するか、トップページへ戻ってください。
```

Recovery link (styled as a button):

```text
トップページへ戻る
```

The link navigates to the image generation screen (`/`) without making a mojica API request. It may be styled like a button, but it remains a link because its purpose is navigation.

English localized examples:

```text
404
```

```text
Page not found
```

```text
Please check the URL or return to the homepage.
```

```text
Back to Home
```

---

# 20. Unexpected Error Screen

This is the ErrorBoundary fallback displayed when an unexpected exception occurs during rendering.

Unlike the 404 screen, it does not display the header or footer. Place ErrorBoundary at the application root outside `I18nProvider`, so it remains usable even if i18n itself causes the exception (see component-design.md). Its copy therefore does not depend on the `I18nProvider` React context; select it from a minimal embedded dictionary. Derive supported locales from the dictionary keys and resolve the locale in this order: the value stored in `localStorage`, the browser language, and the default locale `ja`.

Heading:

```text
エラーが発生しました
```

Description:

```text
予期しないエラーが発生しました。ページを再読み込みして、もう一度お試しください。
```

Button:

```text
ページを再読み込み
```

The button performs a normal browser reload equivalent to `window.location.reload()`, not client-side routing. The root ErrorBoundary may be handling a corrupted provider-inclusive React tree, so navigation within the application cannot guarantee recovery from a clean initial state. The click handler does not call the mojica API directly; any communication after the reload follows the normal initial-render process.

English localized examples:

```text
An error occurred
```

```text
Something unexpected happened. Please reload the page and try again.
```

```text
Reload page
```
