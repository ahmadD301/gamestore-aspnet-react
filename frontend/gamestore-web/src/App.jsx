import {
  BrowserRouter,
  Routes,
  Route,
} from "react-router-dom";

import HomePage from "./pages/HomePage";
import LoginPage from "./pages/LoginPage";
import GamesPage from "./pages/GamesPage";
import GameDetailsPage from "./pages/GameDetailsPage";
import AdminDashboardPage from "./pages/AdminDashboardPage";

import AdminGamesPage from "./pages/AdminGamesPage";

import CreateGamePage from "./pages/CreateGamePage";

import EditGamePage from "./pages/EditGamePage";

import ProtectedRoute from "./routes/ProtectedRoute";
import TopNav from "./components/layout/TopNav";

export default function App() {
  return (
    <BrowserRouter>
      <div className="app-shell">
        <TopNav />

        <main className="main-content">
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
                  <AdminDashboardPage />
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
            <Route
              path="/admin/games"
              element={
                <ProtectedRoute
                  roles={["Admin"]}
                >
                  <AdminGamesPage />
                </ProtectedRoute>
              }
            />

            <Route
              path="/admin/games/new"
              element={
                <ProtectedRoute
                  roles={["Admin"]}
                >
                  <CreateGamePage />
                </ProtectedRoute>
              }
            />

            <Route
              path="/admin/games/:id/edit"
              element={
                <ProtectedRoute
                  roles={["Admin"]}
                >
                  <EditGamePage />
                </ProtectedRoute>
              }
            />
          </Routes>
        </main>
      </div>
    </BrowserRouter>
  );
}