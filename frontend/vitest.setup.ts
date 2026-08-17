import '@testing-library/jest-dom/vitest'
import { beforeAll, afterAll, afterEach } from 'vitest'
import { worker } from './src/mocks/browser'

beforeAll(() => worker.start({ onUnhandledRequest: 'bypass' }))
afterEach(() => worker.resetHandlers())
afterAll(() => worker.stop())
