import {
  BrowserRouter,
  Routes,
  Route,
} from "react-router-dom";

import HomePage from "./pages/HomePage";
import LoginPage from "./pages/LoginPage";
import AdminPage from "./pages/AdminPage";
import GamesPage from "./pages/GamesPage";
import GameDetailsPage from "./pages/GameDetailsPage";

import { ProtectedRoute } from "./routes/ProtectedRoute";

export default function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route
          path="/"
          element={<HomePage />}
        />

        <Route
          path="/login"
          element={<LoginPage />}
        />

        <Route
          path="/admin"
          element={
            <ProtectedRoute
              roles={["Admin"]}
            >
              <AdminPage />
            </ProtectedRoute>
          }
        />

        <Route
          path="/games"
          element={<GamesPage />}
        />

        <Route
          path="/games/:id"
          element={<GameDetailsPage />}
        />
      </Routes>
    </BrowserRouter>
  );
}