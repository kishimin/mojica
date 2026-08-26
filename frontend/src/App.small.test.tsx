import { render, screen } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import { describe, expect, test } from 'vitest'
import App from './App'

describe('App', () => {
  test('increments the counter each time the button is clicked', async () => {
    const user = userEvent.setup()
    render(<App />)

    const button = screen.getByRole('button', { name: 'Count is 0' })
    await user.click(button)

    expect(screen.getByRole('button', { name: 'Count is 1' })).toBeInTheDocument()
  })
})
