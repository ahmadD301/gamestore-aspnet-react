import {
  Navigate,
  Outlet,
  useLocation,
} from "react-router-dom";

import { useAuth }
  from "../auth/AuthContext";

export default function ProtectedRoute({
  roles,
  children,
}) {
  const {
    accessToken,
    user,
    isLoading,
  } = useAuth();

  const location =
    useLocation();

  if (isLoading) {
    return (
      <div className="flex items-center justify-center min-h-screen">
        Loading...
      </div>
    );
  }

  if (!accessToken) {
    return (
      <Navigate
        to="/login"
        replace
        state={{
          from: location,
        }}
      />
    );
  }

  if (
    roles &&
    !roles.some((role) =>
      user?.roles?.includes(role)
    )
  ) {
    return (
      <Navigate
        to="/"
        replace
      />
    );
  }

  if (children) {
    return children;
  }

  return <Outlet />;
}