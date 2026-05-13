/* eslint-disable react-refresh/only-export-components */
import { createContext, useContext, useMemo, useState } from "react";
import { setupInterceptors } from "../api/setupInterceptors";
import { parseJwt } from "../utils/jwt";
const AuthContext = createContext(null);

export function AuthProvider({ children }) {
  const [accessToken, setAccessToken] = useState(null);

  const [refreshToken, setRefreshToken] = useState(null);

  const [user, setUser] = useState(null);

  const login = (authResponse) => {
    setAccessToken(authResponse.accessToken);

    setRefreshToken(authResponse.refreshToken);

    const jwtPayload = parseJwt(authResponse.accessToken);

    const roles =
      jwtPayload[
        "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
      ];

    setUser({
      email: authResponse.email,
      userName: authResponse.userName,
      roles: Array.isArray(roles) ? roles : roles ? [roles] : [],
    });
  };

  const logout = () => {
    setAccessToken(null);

    setRefreshToken(null);

    setUser(null);
  };

  const value = useMemo(
    () => ({
      accessToken,
      refreshToken,
      user,
      login,
      logout,
    }),
    [accessToken, refreshToken, user],
  );
    setupInterceptors(() => accessToken);
  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
}

export function useAuth() {
  const context = useContext(AuthContext);

  if (!context) {
    throw new Error("useAuth must be used inside AuthProvider");
  }
   
  return context;
}
