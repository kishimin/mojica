import { test, expect } from '@playwright/test'

test('the app loads and the counter button is visible', async ({ page }) => {
  await page.goto('/')
  await expect(page.getByRole('button', { name: 'Count is 0' })).toBeVisible()
})
