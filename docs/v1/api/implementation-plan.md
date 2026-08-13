# mojica API Implementation Plan

## 1. Purpose

This document defines the branch plan and implementation order for delivering the
API described in [`api.md`](./api.md).

The plan preserves the dependency direction defined by the Model, Port, Service,
Adapter, and Controller design documents. It also keeps every branch small enough
to review and requires each implementation branch to finish its own TDD cycle
before it is merged.

## 2. Delivery Principles

- Merge `feat/setup-aspnet-core-tdd` before starting this plan.
- Create each branch from the latest commit containing all of its prerequisites.
- Do not place HTTP or Glyph Forge concerns in the Domain Model.
- Keep the dependency direction `Controller -> Service -> Port <- Adapter`.
- Add one behavior at a time with Red, Green, and Refactor commits.
- Keep all tests green at every branch boundary.
- Require at least 80% coverage for every metric reported in the coverage summary.
- Merge independent branches in any order within the same parallel group.
- Rebase an unpublished branch onto its updated base before requesting review when
  another prerequisite was merged first.

## 3. Dependency Overview

```text
ASP.NET Core and test foundation
              |
              v
Shared validation contracts
              |
              +-------------------------------+
              |               |               |
              v               v               v
       Text models       Image type       Color models
              \               |               /
               +--------------+--------------+
                              |
                              v
                 Image generation request
                              |
                              v
             Port -> Service -> Controller
               ^                    |
               |                    v
       Glyph Forge Adapter      HTTP responses
               ^                    ^
               |                    |
       Client and mappings     API cross-cutting concerns
```

## 4. Branch Plan

### Phase 0: Foundation

| Order | Branch | Responsibility | Prerequisites |
| ---: | --- | --- | --- |
| 0 | `feat/setup-aspnet-core-tdd` | Provide the ASP.NET Core solution, test projects, CI execution, test reporting, and coverage reporting. | None |

This branch already exists and is the required base for all branches below.

### Phase 1: Domain Building Blocks

| Order | Branch | Responsibility | Prerequisites |
| ---: | --- | --- | --- |
| 1 | `feat/add-model-validation-errors` | Implement language-independent validation reasons, fields, and errors shared by Domain Models. | Phase 0 |
| 2A | `feat/add-image-type-model` | Implement the closed set of `standard`, `x-background`, and `x-icon` image types. | Branch 1 |
| 2B | `feature/add-render-text-model` | Implement required, grapheme-count, whitespace, and control-character rules for render text. | Branch 1 |
| 2C | `feature/add-pattern-character-model` | Implement required, grapheme-count, and control-character rules for pattern characters. | Branch 1 |
| 2D | `feature/add-rgb-color-model` | Implement valid RGB component values. | Branch 1 |
| 3 | `feature/add-hex-color-model` | Implement `#RRGGBB` validation and conversion to `RgbColor`. | Branch 2D |
| 4 | `feature/add-image-generation-request` | Combine validated value objects and enforce the invariant that foreground and background characters cannot both contain only whitespace. Do not implement HTTP DTO mapping or the endpoint in this branch. | Branches 2A, 2B, 2C, and 3 |

Branches 2A through 2D can be developed in parallel. Branch 3 can begin as soon as
2D is merged. Branch 4 is the synchronization point for the complete Domain input.

Branch 4 may add skipped test-plan scaffolds for responsibilities discovered while
implementing the Domain aggregate, but it does not implement those outer boundaries.
`ImageGenerationRequestMapperSmallTests` is implemented by branch 7B, and
`ImageGenerationEndpointMediumTests` is implemented by branch 11.

`GeneratedImage` is not assigned a new branch because it is already implemented in
the foundation branch.

### Phase 2: Application Boundary and Use Case

| Order | Branch | Responsibility | Prerequisites |
| ---: | --- | --- | --- |
| 5 | `feature/add-image-generation-port` | Define `ImageGenerationPort`, `GeneratedImageData`, and safe, language-independent Port errors. | Branch 4 |
| 6 | `feature/add-image-generation-service` | Invoke the Port once, propagate failures, and create the final `GeneratedImage` with a safe UUID-based filename. | Branch 5 |

The Service depends only on the Port contract. It must be testable without an HTTP
client or a running Glyph Forge instance.

### Phase 3: Independent HTTP and Infrastructure Contracts

The following groups can progress in parallel after their listed prerequisites are
available.

| Order | Branch | Responsibility | Prerequisites |
| ---: | --- | --- | --- |
| 7A | `feature/add-api-error-localization` | Provide Japanese and English public messages, language selection, and Japanese fallback behavior. | Branch 1 |
| 7B | `feature/add-image-api-contracts` | Define request DTOs, implement the input Mapper from DTO values to `ImageGenerationRequest`, and define stable public success and error response contracts without treating DTOs as Domain Models. Implement `ImageGenerationRequestMapperSmallTests`, including preservation of validation reasons and assignment of request-attribute targets. | Branch 4 |
| 7C | `feature/configure-glyph-forge-client` | Define validated Glyph Forge client options, the HTTP client registration, and timeout configuration. | Phase 0 |
| 7D | `feature/add-api-rate-limiting` | Configure local API rate limiting and generation of a valid `Retry-After` response. | Phase 0 |

The input Mapper belongs to branch 7B because it owns the DTO-to-Domain attribute
context required by ADR-0022. It can be implemented after branch 4 without waiting
for the Port or Service. HTTP response behavior and downstream-call suppression
remain outside this Mapper branch.

### Phase 4: Glyph Forge Adapter

| Order | Branch | Responsibility | Prerequisites |
| ---: | --- | --- | --- |
| 8A | `feature/add-glyph-forge-request-mapping` | Select the endpoint from `ImageType` and convert validated Domain values, including HEX-to-RGB output, into Glyph Forge request DTOs. | Branches 4 and 7C |
| 8B | `feature/add-glyph-forge-response-mapping` | Validate successful PNG responses and map rate limits, timeouts, unavailable responses, invalid responses, and other failures to Port errors. | Branches 5 and 7C |
| 9 | `feature/add-glyph-forge-adapter` | Implement `ImageGenerationPort` with the configured HTTP client and the tested request and response mappings. | Branches 8A and 8B |

Branches 8A and 8B can be developed in parallel. The Adapter branch is limited to
the communication orchestration that connects those mappings to the Port.

### Phase 5: Public API

| Order | Branch | Responsibility | Prerequisites |
| ---: | --- | --- | --- |
| 10 | `feature/add-api-error-mapping` | Map malformed requests, validation failures, rate limits, unexpected failures, upstream failures, and timeouts to 400, 422, 429, 500, 502, and 504 responses. | Branches 5, 7A, 7B, and 7D |
| 11 | `feature/add-image-generation-endpoint` | Drive `POST /images` from `ImageGenerationEndpointMediumTests` and the remaining cross-layer API contract tests, collect validation errors, return 422 without invoking the Service for rejected requests, invoke the Service for valid requests, return raw PNG data, and wire all production dependencies. | Branches 6, 7B, 7D, 9, and 10 |

The endpoint is intentionally last among production branches. This prevents the
Controller from temporarily owning Domain, Service, or Adapter responsibilities.
Its TDD test list covers localization, all documented status codes, PNG responses,
rate-limit headers, and the rule that rejected requests never reach Glyph Forge.
Keeping these tests with the endpoint ensures that each contract test fails before
the corresponding endpoint behavior is implemented.

`ImageGenerationEndpointMediumTests` owns the observable HTTP form of the deferred
request-failure plan: an invalid but parseable request returns 422 with the affected
field and does not invoke the image generation Service. It is not implemented in
branch 4 or branch 7B.

## 5. Recommended Merge Sequence

The branch numbers express dependencies, not a requirement to serialize independent
work. A low-conflict merge sequence is:

1. Merge the foundation and shared validation branches.
2. Develop and merge branches 2A through 2D in parallel.
3. Merge the HEX color and aggregate request branches.
4. Merge the Port and Service branches.
5. Develop the four branches in group 7 in parallel.
6. Develop the two mapping branches in group 8 in parallel, then merge the Adapter.
7. Merge API error mapping, followed by the contract-test-driven endpoint.

## 6. Branch Definition of Done

Every branch is complete only when:

- Its behavior is represented by an explicit test list.
- Each behavior has been observed failing before its implementation is added.
- All new and existing tests pass.
- Refactoring is performed only while the test suite is green.
- The reported line, branch, method, and other configured coverage metrics are each
  at least 80%.
- Public errors contain no credentials, internal URLs, stack traces, or unnecessary
  upstream implementation details.
- The branch contains only its declared responsibility and required tests.
- The design documents remain consistent with the implemented behavior.

## 7. Resulting API Behavior

After all branches are merged, `POST /images` will:

1. Parse the JSON request.
2. Select Japanese or English error messages, falling back to Japanese.
3. Validate all input and collect errors when possible.
4. Reject invalid requests without calling the Service or Glyph Forge.
5. Apply the local API rate limit.
6. Route the validated request to the correct Glyph Forge endpoint.
7. Convert HEX colors to RGB at the Adapter boundary.
8. Return generated PNG data on success.
9. Return stable, localized error contracts for documented failures.
10. Preserve `Retry-After` information when a safe retry period is available.
