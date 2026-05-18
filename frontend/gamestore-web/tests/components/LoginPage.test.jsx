import {
  render,
  screen,
}
from "@testing-library/react";

import userEvent
from "@testing-library/user-event";

import { MemoryRouter }
from "react-router-dom";

import { vi }
from "vitest";

import LoginPage
from "../../src/pages/LoginPage";

import { AuthContext }
from "../../src/auth/AuthContext";

// eslint-disable-next-line no-undef
describe("LoginPage", () => {
  // eslint-disable-next-line no-undef
  it(
    "shows validation errors",
    async () => {
      const login = vi.fn()
        .mockRejectedValue({
          response: {
            data: {
              message:
                "Email is required",
            },
          },
        });

      render(
        <AuthContext.Provider
          value={{
            login,
          }}
        >
          <MemoryRouter>
            <LoginPage />
          </MemoryRouter>
        </AuthContext.Provider>
      );

      const button =
        screen.getByRole(
          "button",
          {
            name: /login/i,
          }
        );

      const user =
        userEvent.setup();

      await user.click(button);

      // eslint-disable-next-line no-undef
      expect(
        await screen.findByText(
          /email is required/i
        )
      ).toBeInTheDocument();
    }
  );
});