# mojica MVP API Specification

## Overview

The mojica backend is implemented using ASP.NET Core.

The backend receives input from the frontend, sends requests to the image generation API, "Glyph Forge API," and returns the generated image to the client.

The frontend uses a single image generation endpoint. The backend is responsible for routing requests to the appropriate Glyph Forge API endpoint based on the image type.

The frontend uses a color picker and handles colors in HEX format. The mojica API also accepts colors in HEX format. ASP.NET Core converts the HEX values to RGB before sending them to the Glyph Forge API.

The mojica API supports internationalization (i18n), allowing error messages returned to the client to be switched between Japanese and English.

---

# System Architecture

```text
Frontend
    │
    │ POST /images
    │ Color: HEX
    │ Accept-Language: ja / en
    ▼
ASP.NET Core API
    │
    ├── Language detection
    ├── Request validation
    ├── HEX → RGB conversion
    ├── Rate limiting
    └── Routing based on type
            │
            ▼
      Glyph Forge API
        ├── POST /images
        ├── POST /images/background
        └── POST /images/x-icon
            │
            ▼
          PNG image
            │
            ▼
       ASP.NET Core API
            │
            ▼
         Frontend
```

---

# API Endpoints

| Method | Endpoint  | Description                |
| ------ | --------- | -------------------------- |
| POST   | `/images` | Generates a text art image |

---

# Image Generation API

## Endpoint

```http
POST /images
```

## Description

Generates a text art image based on the provided input.

ASP.NET Core sends a request to the appropriate Glyph Forge API endpoint based on the value of `type`.

When image generation succeeds, the generated PNG image is returned in the response.

---

# Request

## Headers

| Header            | Required | Description                                    |
| ----------------- | :------: | ---------------------------------------------- |
| `Content-Type`    |   Yes    | `application/json`                             |
| `Accept-Language` |    No    | Specifies the language used for error messages |

The following values are supported for `Accept-Language`.

| Value | Language |
| ----- | -------- |
| `ja`  | Japanese |
| `en`  | English  |

Japanese:

```http
Accept-Language: ja
```

English:

```http
Accept-Language: en
```

If `Accept-Language` is not specified, Japanese is used by default.

If an unsupported language is specified, the API falls back to Japanese.

---

## Body

| Field               | Type   | Required | Description                              |
| ------------------- | ------ | :------: | ---------------------------------------- |
| type                | enum   |   Yes    | Type of image to generate                |
| text                | string |   Yes    | Text to render                           |
| foregroundCharacter | string |   Yes    | Character used to render the text        |
| foregroundColor     | string |   Yes    | Foreground character color in HEX format |
| backgroundCharacter | string |   Yes    | Character used to fill the background    |
| backgroundColor     | string |   Yes    | Background character color in HEX format |

## type

| Value          | Description        |
| -------------- | ------------------ |
| `standard`     | Standard image     |
| `x-background` | X background image |
| `x-icon`       | X profile image    |

## Request Example

```http
POST /images
Content-Type: application/json
Accept-Language: ja
```

```json
{
  "type": "x-icon",
  "text": "KA",
  "foregroundCharacter": "🌻",
  "foregroundColor": "#FFD400",
  "backgroundCharacter": "☀",
  "backgroundColor": "#FF69B4"
}
```

---

# Glyph Forge API Integration

## Endpoint Routing

ASP.NET Core selects the appropriate Glyph Forge API endpoint based on the value of `type`.

| type           | Glyph Forge API           |
| -------------- | ------------------------- |
| `standard`     | `POST /images`            |
| `x-background` | `POST /images/background` |
| `x-icon`       | `POST /images/x-icon`     |

By handling this routing in the backend, the frontend does not need to be aware of the Glyph Forge API endpoint structure.

---

# Color Conversion

The frontend uses a color picker and obtains colors in HEX format.

For example, the following value is sent to the mojica API:

```text
#FF69B4
```

The mojica API converts the received HEX color to RGB in ASP.NET Core.

```text
#FF69B4

↓

R: 255
G: 105
B: 180
```

The converted RGB values are then sent to the Glyph Forge API.

By handling this conversion in the backend, the frontend does not need to be aware of the color format required by the Glyph Forge API.

---

# Internationalization (i18n)

The mojica API supports localized error messages returned to the client.

The MVP supports Japanese and English.

## Supported Languages

| Language Code | Language |
| ------------- | -------- |
| `ja`          | Japanese |
| `en`          | English  |

## Language Selection

The client specifies the language using the HTTP `Accept-Language` header.

```http
Accept-Language: ja
```

or:

```http
Accept-Language: en
```

If `Accept-Language` is not specified, Japanese is used by default.

If an unsupported language is specified, the API falls back to Japanese.

## Error Codes and Messages

`code` and `field` are language-independent fixed values.

Only `message` and `errors[].message` are localized according to the requested language.

### Japanese

```json
{
  "code": "VALIDATION_ERROR",
  "message": "入力内容に誤りがあります。",
  "errors": [
    {
      "field": "foregroundColor",
      "message": "HEXカラー形式（#RRGGBB）で指定してください。"
    }
  ]
}
```

### English

```json
{
  "code": "VALIDATION_ERROR",
  "message": "The input contains validation errors.",
  "errors": [
    {
      "field": "foregroundColor",
      "message": "The value must be specified in HEX color format (#RRGGBB)."
    }
  ]
}
```

This allows the frontend to identify errors using `code` and `field` independently of the selected display language.

---

# Response

## 200 OK

Returned when image generation succeeds.

This API does not create a persistent image resource on the server. Instead, the PNG image is generated on demand and returned directly in the response. Therefore, `200 OK` is used instead of `201 Created`.

| Field         | Value               |
| ------------- | ------------------- |
| Status Code   | `200 OK`            |
| Content-Type  | `image/png`         |
| Response Body | Generated PNG image |

ASP.NET Core returns the PNG image received from the Glyph Forge API to the client.

---

# Error Responses

Error responses are returned in JSON format.

Error messages are returned in Japanese or English according to the `Accept-Language` header.

`code` and `field` remain language-independent fixed values, while `message` and `errors[].message` are localized.

---

## 400 Bad Request

Returned when the HTTP request cannot be interpreted correctly.

Typical cases include:

- Invalid JSON syntax
- The request body cannot be parsed as JSON
- The request does not match the expected request format

### Japanese

```json
{
  "code": "BAD_REQUEST",
  "message": "リクエストの形式が正しくありません。"
}
```

### English

```json
{
  "code": "BAD_REQUEST",
  "message": "The request format is invalid."
}
```

---

## 422 Unprocessable Entity

Returned when the request can be parsed successfully but the input values do not satisfy the API requirements.

Typical cases include:

- A required field is missing or empty
- An undefined value is specified for `type`
- A color is not specified in HEX format
- A string does not satisfy the allowed constraints

### Required Field Error

Japanese:

```json
{
  "code": "VALIDATION_ERROR",
  "message": "入力内容に誤りがあります。",
  "errors": [
    {
      "field": "text",
      "message": "描画する文字列は必須です。"
    }
  ]
}
```

English:

```json
{
  "code": "VALIDATION_ERROR",
  "message": "The input contains validation errors.",
  "errors": [
    {
      "field": "text",
      "message": "The text field is required."
    }
  ]
}
```

### Invalid Color Format

Japanese:

```json
{
  "code": "VALIDATION_ERROR",
  "message": "入力内容に誤りがあります。",
  "errors": [
    {
      "field": "foregroundColor",
      "message": "HEXカラー形式（#RRGGBB）で指定してください。"
    }
  ]
}
```

English:

```json
{
  "code": "VALIDATION_ERROR",
  "message": "The input contains validation errors.",
  "errors": [
    {
      "field": "foregroundColor",
      "message": "The value must be specified in HEX color format (#RRGGBB)."
    }
  ]
}
```

### Invalid `type`

Japanese:

```json
{
  "code": "VALIDATION_ERROR",
  "message": "入力内容に誤りがあります。",
  "errors": [
    {
      "field": "type",
      "message": "standard、x-background、x-iconのいずれかを指定してください。"
    }
  ]
}
```

English:

```json
{
  "code": "VALIDATION_ERROR",
  "message": "The input contains validation errors.",
  "errors": [
    {
      "field": "type",
      "message": "The value must be one of: standard, x-background, or x-icon."
    }
  ]
}
```

If multiple fields contain validation errors, all validation errors are included in the `errors` array.

---

## 429 Too Many Requests

Returned in either of the following cases:

- The mojica API rate limit has been exceeded
- The Glyph Forge API rate limit has been exceeded

Because image generation is relatively resource-intensive, the mojica API also applies rate limiting to protect both the mojica API and the Glyph Forge API from excessive image generation requests.

If the mojica API's own rate limit is exceeded, it returns `429 Too Many Requests` without calling the Glyph Forge API.

If the Glyph Forge API returns `429 Too Many Requests`, the mojica API also returns `429 Too Many Requests` to the client.

### Japanese

```json
{
  "code": "RATE_LIMIT_EXCEEDED",
  "message": "リクエスト回数の上限に達しました。時間をおいて再度お試しください。"
}
```

### English

```json
{
  "code": "RATE_LIMIT_EXCEEDED",
  "message": "The request limit has been exceeded. Please try again later."
}
```

When the retry timing is known, the `Retry-After` header is returned.

If the Glyph Forge API provides a `Retry-After` header, the mojica API takes that value into account when setting its own response header.

```http
HTTP/1.1 429 Too Many Requests
Retry-After: 60
Content-Type: application/json
```

---

## 500 Internal Server Error

Returned when an unexpected error occurs within the ASP.NET Core application.

### Japanese

```json
{
  "code": "INTERNAL_SERVER_ERROR",
  "message": "画像生成中に予期しないエラーが発生しました。"
}
```

### English

```json
{
  "code": "INTERNAL_SERVER_ERROR",
  "message": "An unexpected error occurred while generating the image."
}
```

Internal information such as exception messages and stack traces must not be included in the response.

---

## 502 Bad Gateway

Returned when an issue occurs while communicating with the Glyph Forge API or when a valid image cannot be obtained from it.

Typical cases include:

- The Glyph Forge API returns an error
- The Glyph Forge API returns an unexpected response
- Communication with the Glyph Forge API fails

A `429 Too Many Requests` response or timeout from the Glyph Forge API is handled separately as `429 Too Many Requests` or `504 Gateway Timeout`, respectively.

### Japanese

```json
{
  "code": "IMAGE_GENERATION_FAILED",
  "message": "画像の生成に失敗しました。時間をおいて再度お試しください。"
}
```

### English

```json
{
  "code": "IMAGE_GENERATION_FAILED",
  "message": "Image generation failed. Please try again later."
}
```

Internal error details from the Glyph Forge API must not be exposed directly to the client.

---

## 504 Gateway Timeout

Returned when a response cannot be obtained from the Glyph Forge API within the configured timeout period.

### Japanese

```json
{
  "code": "IMAGE_GENERATION_TIMEOUT",
  "message": "画像の生成に時間がかかっています。時間をおいて再度お試しください。"
}
```

### English

```json
{
  "code": "IMAGE_GENERATION_TIMEOUT",
  "message": "Image generation is taking too long. Please try again later."
}
```

---

# HTTP Status Codes

| Status                      | Description                                        |
| --------------------------- | -------------------------------------------------- |
| `200 OK`                    | Image generation succeeded                         |
| `400 Bad Request`           | Invalid request format                             |
| `422 Unprocessable Entity`  | Input validation failed                            |
| `429 Too Many Requests`     | mojica API or Glyph Forge API rate limit exceeded  |
| `500 Internal Server Error` | Unexpected error within the mojica API             |
| `502 Bad Gateway`           | Glyph Forge API request or image generation failed |
| `504 Gateway Timeout`       | Glyph Forge API request timed out                  |

---

# Rate Limiting

ASP.NET Core applies rate limiting to prevent excessive image generation requests.

The Glyph Forge API also has its own rate limit.

Therefore, the following two rate limits must be considered:

- The mojica API's own rate limit
- The Glyph Forge API rate limit

If the mojica API's own rate limit is exceeded, the mojica API returns `429 Too Many Requests` without calling the Glyph Forge API.

If the Glyph Forge API returns `429 Too Many Requests`, the mojica API also returns `429 Too Many Requests` to the client.

If the Glyph Forge API provides a `Retry-After` header, the mojica API takes that value into account when returning the response to the client.

As a general rule, the mojica API rate limit should be configured so that requests are restricted before the Glyph Forge API rate limit is reached.

The exact request limit and time window for the mojica API will be determined based on the Glyph Forge API rate limit and the production environment.

---

# Backend Responsibilities for the MVP

The ASP.NET Core backend is responsible for:

- Receiving image generation requests from the frontend
- Validating requests
- Detecting Japanese or English based on `Accept-Language`
- Localizing error messages in Japanese and English
- Falling back to Japanese when `Accept-Language` is not specified
- Falling back to Japanese when an unsupported language is specified
- Keeping error codes and field names language-independent
- Converting HEX colors to RGB
- Selecting the appropriate Glyph Forge API endpoint based on `type`
- Sending requests to the Glyph Forge API
- Returning PNG images received from the Glyph Forge API
- Mapping Glyph Forge API errors to appropriate mojica API responses
- Handling Glyph Forge API rate limits
- Handling timeouts
- Applying the mojica API's own rate limiting
- Preventing internal error information from being exposed to the client
