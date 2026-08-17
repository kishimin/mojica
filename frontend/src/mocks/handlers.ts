import type { HttpHandler } from 'msw'

// Request handlers are added here as API integration work adds real
// fetch calls to mock (see https://mswjs.io/docs/basics/request-handler).
export const handlers: HttpHandler[] = []
