import {
  render,
  screen,
}
from "@testing-library/react";

import {
  MemoryRouter,
  Routes,
  Route,
}
from "react-router-dom";

import ProtectedRoute
from "../../src/routes/ProtectedRoute";

import {
  AuthContext,
}
from "../../src/auth/AuthContext";

// eslint-disable-next-line no-undef
describe(
  "ProtectedRoute",
  () => {
    // eslint-disable-next-line no-undef
    it(
      "redirects unauthenticated users",
      () => {
        render(
          <AuthContext.Provider
            value={{
              user: null,
              isAuthReady:
                true,
            }}
          >
            <MemoryRouter
              initialEntries={[
                "/admin",
              ]}
            >
              <Routes>
                <Route
                  path="/login"
                  element={
                    <div>
                      Login Page
                    </div>
                  }
                />

                <Route
                  path="/admin"
                  element={
                    <ProtectedRoute
                      roles={[
                        "Admin",
                      ]}
                    >
                      <div>
                        Admin
                      </div>
                    </ProtectedRoute>
                  }
                />
              </Routes>
            </MemoryRouter>
          </AuthContext.Provider>
        );

        // eslint-disable-next-line no-undef
        expect(
          screen.getByText(
            "Login Page"
          )
        ).toBeInTheDocument();
      }
    );
  }
);