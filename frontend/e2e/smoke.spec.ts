import { test, expect } from '@playwright/test'

test('the app loads and renders its heading', async ({ page }) => {
  await page.goto('/')
  await expect(page.getByRole('heading', { name: 'Get started' })).toBeVisible()
})
