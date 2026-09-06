<div id="top"></div>

# mojica

Generate text-art PNG images through a React frontend and an ASP.NET Core API.

## Tech Stack

<p style="display: inline">
  <img src="https://img.shields.io/badge/-TypeScript-3178C6.svg?logo=typescript&style=for-the-badge&logoColor=white">
  <img src="https://img.shields.io/badge/-React-61DAFB.svg?logo=react&style=for-the-badge&logoColor=black">
  <img src="https://img.shields.io/badge/-.NET-512BD4.svg?logo=dotnet&style=for-the-badge&logoColor=white">
  <img src="https://img.shields.io/badge/-Vite-646CFF.svg?logo=vite&style=for-the-badge&logoColor=white">
  <img src="https://img.shields.io/badge/-Playwright-2EAD33.svg?logo=playwright&style=for-the-badge&logoColor=white">
  <img src="https://img.shields.io/badge/-Docker-2496ED.svg?logo=docker&style=for-the-badge&logoColor=white">
</p>

## Table of Contents

1. [About the Project](#about-the-project)
2. [Environment](#environment)
3. [Directory Structure](#directory-structure)
4. [Getting Started](#getting-started)
5. [Usage](#usage)
6. [API Endpoints](#api-endpoints)
7. [Available Commands](#available-commands)
8. [Troubleshooting](#troubleshooting)

## About the Project

mojica contains three cooperating parts:

- **mojica Web** — A React single-page application for entering text, characters, colors, and an image type.
- **mojica API** — An ASP.NET Core API that validates requests, localizes errors, converts HEX colors to RGB, and returns generated PNG data.
- **Glyph Forge** — The image-generation service called by the mojica API for `standard`, `x-background`, and `x-icon` images.

The frontend calls only the mojica API. The backend selects the Glyph Forge endpoint, applies validation and rate limiting, and returns the generated image directly rather than persisting an image resource.

In the release architecture, the frontend is hosted on Cloudflare Pages and the two APIs run as Docker containers on Sakura Cloud. Local development can run the frontend and backend independently.

<p align="right">(<a href="#top">back to top</a>)</p>

## Environment

| Language / Framework | Version |
| -------------------- | ------- |
| Bun | 1.3.13 |
| TypeScript | ~6.0.2 |
| React | ^19.2.8 |
| Vite | 7.3.1 |
| .NET | 8.0.x |
| ASP.NET Core | `net8.0` |

Frontend dependencies and scripts are declared in `frontend/package.json` and resolved in `frontend/bun.lock`. Backend package references are declared in `backend/Mojica.Api/Mojica.Api.csproj` and `backend/Mojica.Api.Tests/Mojica.Api.Tests.csproj`.

<p align="right">(<a href="#top">back to top</a>)</p>

## Directory Structure

```text
.
├── .github
│   └── workflows
├── backend
│   ├── Mojica.Api
│   ├── Mojica.Api.Tests
│   ├── Dockerfile
│   └── Mojica.Backend.sln
├── docs
│   └── v1
├── frontend
│   ├── e2e
│   ├── public
│   ├── src
│   ├── package.json
│   ├── playwright.config.ts
│   └── vite.config.ts
├── cspell.json
└── README.md
```

### Main Directories

| Directory | Description |
| --------- | ----------- |
| `backend/Mojica.Api` | ASP.NET Core API, domain models, validation, Glyph Forge integration, and OpenAPI configuration. |
| `backend/Mojica.Api.Tests` | xUnit tests for API contracts, domain behavior, mappings, infrastructure, and endpoints. |
| `frontend/src` | React application, routes, providers, components, API client, and image-generation feature. |
| `frontend/e2e` | Playwright fixtures, Page Objects, selectors, and browser tests. |
| `docs/v1` | Requirements, API/UI specifications, architecture, release, and implementation plans. |
| `.github/workflows` | Push, pull-request, and nightly CI workflows for sized tests, coverage, E2E, and reports. |

<p align="right">(<a href="#top">back to top</a>)</p>

## Getting Started

### Prerequisites

Install Bun 1.3.13 and .NET 8 SDK. Docker is required when running a local Glyph Forge container.

### Clone the Repository

```bash
git clone https://github.com/kishimin/mojica.git
cd mojica
```

### Install Frontend Dependencies

```powershell
cd frontend
bun install --frozen-lockfile
```

### Configure the Frontend

Create `frontend/.env.local` and point the generated API client at the local backend:

```dotenv
VITE_API_URL=http://localhost:5025
```

### Start the Backend

From the repository root, start the Development profile:

```powershell
dotnet run --project backend/Mojica.Api
```

The API listens on `http://localhost:5025` and exposes Swagger UI at `http://localhost:5025/swagger`. Image generation also requires a reachable Glyph Forge service configured through the backend `GlyphForge` settings.

### Start the Frontend

In a second terminal:

```powershell
cd frontend
bun run dev
```

Open:

```text
http://localhost:5173
```

### Run the Production Container

Build and run the backend container from the `backend` directory:

```powershell
cd backend
docker build -t mojica-api:local .
docker run --rm -p 8080:8080 mojica-api:local
```

Confirm that the container is healthy:

```powershell
Invoke-WebRequest http://localhost:8080/health
```

<p align="right">(<a href="#top">back to top</a>)</p>

## Usage

### Generate a PNG through the API

With the backend and Glyph Forge running, send a JSON request to the image-generation endpoint:

```powershell
$body = @{
  type = "standard"
  text = "KA"
  foregroundCharacter = "A"
  foregroundColor = "#FFD400"
  backgroundCharacter = "B"
  backgroundColor = "#FF69B4"
} | ConvertTo-Json

Invoke-WebRequest `
  -Uri http://localhost:5025/images `
  -Method Post `
  -ContentType "application/json" `
  -Headers @{ "Accept-Language" = "ja" } `
  -Body $body `
  -OutFile .\mojica-standard.png
```

The successful response is a generated `image/png`. Error responses use JSON and are localized by the `Accept-Language` header (`ja` or `en`).

<p align="right">(<a href="#top">back to top</a>)</p>

## API Endpoints

| Method | Path | Description |
| ------ | ---- | ----------- |
| `GET` | `/health` | Returns the API health status. |
| `POST` | `/images` | Validates an image-generation request and returns a generated PNG. |

### Request Body

| Field | Required | Default | Description |
| ----- | -------- | ------- | ----------- |
| `type` | Yes | - | `standard`, `x-background`, or `x-icon`. |
| `text` | Yes | - | Text to render, up to 64 characters. |
| `foregroundCharacter` | Yes | - | Character(s) used to render the text, up to 128 characters. |
| `foregroundColor` | Yes | - | HEX color in `#RRGGBB` format. |
| `backgroundCharacter` | Yes | - | Character(s) used to fill the surrounding area, up to 128 characters. |
| `backgroundColor` | Yes | - | HEX color in `#RRGGBB` format. |

The API returns `200 OK` with `image/png` on success. It returns `400` for malformed requests, `422` for validation failures, `429` for rate limiting, `500` for unexpected backend failures, `502` for Glyph Forge failures, and `504` for Glyph Forge timeouts. `foregroundCharacter` and `backgroundCharacter` may individually contain only whitespace, but they cannot both be whitespace-only. The `Retry-After` header is returned when retry timing is available.

<p align="right">(<a href="#top">back to top</a>)</p>

## Available Commands

| Command | Working directory | Description |
| ------- | ----------------- | ----------- |
| `dotnet restore Mojica.Backend.sln` | `backend` | Restore backend dependencies. |
| `dotnet build Mojica.Backend.sln --configuration Release` | `backend` | Build the API and test projects. |
| `dotnet run --project backend/Mojica.Api` | repository root | Start the ASP.NET Core API. |
| `dotnet test backend/Mojica.Api.Tests/Mojica.Api.Tests.csproj` | repository root | Run backend tests. |
| `dotnet test backend/Mojica.Api.Tests/Mojica.Api.Tests.csproj --collect:"XPlat Code Coverage"` | repository root | Collect backend coverage. |
| `bun install --frozen-lockfile` | `frontend` | Install the locked frontend dependencies. |
| `bun run dev` | `frontend` | Start the Vite development server. |
| `bun run build` | `frontend` | Type-check and build the frontend. |
| `bun run typecheck` | `frontend` | Run the TypeScript compiler without emitting files. |
| `bun run lint` | `frontend` | Run Oxlint and ESLint. |
| `bun run test` | `frontend` | Run the frontend Vitest project. |
| `bun run test:small` | `frontend` | Run Small frontend tests with coverage. |
| `bun run test:medium` | `frontend` | Run Medium frontend tests with coverage. |
| `bun run test:large` | `frontend` | Run Large frontend tests with coverage. |
| `bun run test:storybook` | `frontend` | Run Storybook tests. |
| `bun run e2e` | `frontend` | Run Playwright E2E tests. |
| `bun run storybook` | `frontend` | Start Storybook on port 6006. |
| `bun run generate-api` | `frontend` | Regenerate the frontend API client with Orval. |

<p align="right">(<a href="#top">back to top</a>)</p>

## Troubleshooting

### `curl: (7) Failed to connect to 127.0.0.1 port 5063`

The E2E workflow expects the API process to be running on port 5063. Start the backend with the same URL binding before running a local E2E flow:

```powershell
$env:ASPNETCORE_ENVIRONMENT = "Development"
$env:ASPNETCORE_URLS = "http://127.0.0.1:5063"
dotnet run --project backend/Mojica.Api
```

### `Set RELEASE_E2E_BASE_URL to run against the deployed service.`

Release E2E tests intentionally skip when no deployed frontend URL is provided. Set the variable before running the release suite:

```powershell
$env:RELEASE_E2E_BASE_URL = "https://mojica.pages.dev/"
cd frontend
bun run e2e
```

<p align="right">(<a href="#top">back to top</a>)</p>
