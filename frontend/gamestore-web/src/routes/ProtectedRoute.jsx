import { Navigate } from "react-router-dom";
import { useAuth } from "../auth/AuthContext";

export function ProtectedRoute({
  children,
  roles = [],
}) {
  const { accessToken, user } = useAuth();

  if (!accessToken) {
    return <Navigate to="/login" replace />;
  }

  if (
    roles.length > 0 &&
    !roles.some((role) =>
      user?.roles?.includes(role)
    )
  ) {
    return <Navigate to="/" replace />;
  }

  return children;
}