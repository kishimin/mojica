# mojica Frontend Architecture Design

This document defines the hosting, rendering, and BFF/API boundaries used to implement [ui.md](./ui.md) and [component-design.md](./component-design.md). The API contract follows [api.md](../api/api.md).

---

# 1. Requirements, Constraints, and Assumptions

- **Distribution**: Public, without authentication or stored user information (ui.md §17).
- **SEO**: Not required; this image-generation tool does not assume search-engine acquisition.
- **Initial display**: No special requirement beyond keeping the bundle reasonably small.
- **Traffic variation**: No explicit MVP forecast; assume CDN auto-scaling.
- **Data updates**: Static assets only. The sole dynamic operation is user-initiated `POST /images`; initial render fetches no data.
- **Availability, cost, and operations**: Assume no dedicated server-operations team and complete the system with static hosting.
- **Hosting**: A cloud/CDN separate from the ASP.NET Core backend, such as Cloudflare Pages.
- **API communication**: Call the mojica API directly from the browser through CORS, with no BFF or proxy.
- **Routing**: The MVP has `/` and a 404 screen (ui.md §2 and §19; Figma design `mojica / 404 Not Found`). A client-side router is required to recognize unmatched paths.
- **i18n**: Express language through `Accept-Language`, not URLs (ui.md §3 and §13); locale-specific routes are unnecessary.
- **Security**: The frontend stores no secrets and handles no credentials, cookies, or sessions.

---

# 2. Adopted Hosting, Rendering, BFF, and Routing

## Rendering: CSR (Client-Side Rendering)

Rationale:

1. Public delivery without SEO does not require SSR.
2. Although most content is static, the screen's purpose is interactive form submission, so SSG/ISR provides little benefit.
3. Prefer simple development and low hosting overhead.

Implement an SPA with CSR and distribute its build output as static assets.

## Hosting: CDN and static hosting (such as Cloudflare Pages)

Adopt a configuration equivalent to CloudFront + S3 after confirming it provides:

- globally low-latency edge delivery;
- HTTPS by default, custom security headers (for example `_headers`), SPA fallback, atomic deployment, and immediate rollback; and
- no server operations, cold-start management, or scaling design.

Do not use production architecture such as standalone S3 website hosting if it lacks HTTPS, custom headers, or blue/green deployment.

## BFF: none; direct CORS calls from the browser

- No BFF is required to conceal secrets because the frontend handles no API keys or similar secrets.
- Permit the frontend origin in backend CORS configuration.
- Send with `credentials: "omit"`. Cookie-based sessions and their CSRF measures are out of scope.

## Routing

- Use TanStack Router with file-based routing. The Vite plugin (`@tanstack/router-plugin/vite`) generates `src/routeTree.gen.ts` from `src/routes/__root.tsx` and `index.tsx`; never edit the generated file. Define `/` in `index.tsx` and handle every unmatched path with `notFoundComponent` in `__root.tsx` (see component-design.md).
- Enable the hosting platform's SPA fallback so unknown paths return `index.html`; TanStack Router then performs the actual 404 decision and display.
- Keep locale in client state at local-storage key `"locale"` with value `"ja"` or `"en"`, not in the URL, and attach it to API requests as `Accept-Language`.

## Environment configuration (API endpoint resolution)

- CSR cannot resolve server runtime environment variables like SSR. Inject the mojica API base URL at build time, for example through Vite's `import.meta.env.VITE_API_BASE_URL`.
- Build against the appropriate URL for each production or preview environment and map it in hosting deployment settings.
- If operations later require promoting an identical artifact across environments, consider runtime `config.json`; it is unnecessary complexity for the MVP.

---

# 3. Security, Caching, Performance, and Operational Risks

## Security

- **CORS**: Restrict API origins to production and preview frontend domains. Preview hosts commonly allocate dynamic subdomains, so agree with the backend on their allow-list pattern (residual risk).
- Although cookie-free operation needs no CSRF defense, accept only `Content-Type: application/json` to prevent misuse as a simple CORS request.
- Configure CSP, X-Content-Type-Options, Referrer-Policy, and Permissions-Policy at the host. Include the API origin in CSP `connect-src`.
- Do not expose secrets or internal origins to the browser; treat the API base URL as public.

## Caching

- Give hashed JS/CSS static assets a long-lived, immutable-equivalent cache policy.
- Give `index.html` `no-cache` or a short TTL so updates take effect quickly.
- Do not cache `POST /images`. Generated images are not stored server-side (api.md §10), so they are not CDN-cacheable either.

## Performance

- CSR delays initial display until JavaScript runs compared with SSR. One form screen does not justify additional code splitting, but measure the bundle size.
- CDN edge delivery improves static-asset TTFB across regions.

## Operations

- With no server process, no cold-start or scaling design is required.
- Use atomic CDN deployments and immediately roll back to the preceding deployment if a problem occurs.

---

# 4. Verification, Deployment/Rollback, and Residual Risks

## Verification

- In a production-equivalent build, `POST /images` succeeds without CORS errors and preflight `OPTIONS` permits `Content-Type` and `Accept-Language`.
- Hosting security headers appear on actual responses.
- Direct access to `/foo` returns `index.html` through SPA fallback and TanStack Router displays the 404 screen.
- Initial-display performance, including LCP, is acceptable on representative devices and networks.
- After switching language, `Accept-Language`, displayed copy, and API error language agree.
- Errors 400/429/500/502/504 appear as non-field errors consistently with the generated Orval hook's `isError`/`error` contract.

## Deployment and rollback

- Deploy CI-built static assets to the CDN platform.
- Switch deployments atomically and roll back immediately without restarting servers or replacing instances.
- Before production, verify connectivity to the mojica API or staging equivalent from a preview deployment.

## Residual risks

- Backend handling of CORS for preview origins is unresolved.
- Build-time URL injection prevents promotion of one artifact across production, staging, and preview. If operations become more complex, migrate to runtime `config.json`.
- The specific hosting product and contract are undecided.
