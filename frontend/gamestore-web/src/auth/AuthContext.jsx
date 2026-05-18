import {
  createContext,
  useContext,
  useEffect,
  useCallback,
  useRef,
  useMemo,
  useState,
} from "react";

import { apiClient } from "../api/client";
import { setupInterceptors } from "../api/setupInterceptors";

export const AuthContext = createContext(null);

export function AuthProvider({ children }) {
  const [accessToken, setAccessToken] =
    useState(null);

  const [user, setUser] =
    useState(null);

  const [isLoading, setIsLoading] =
    useState(true);

  const didRestoreRef = useRef(false);

  useEffect(() => {
    if (didRestoreRef.current) {
      return;
    }

    didRestoreRef.current = true;

    const restoreSession = async () => {
      try {
        const response =
          await apiClient.post(
            "/api/auth/refresh"
          );

        setAccessToken(
          response.data.accessToken
        );

        setUser({
          email: response.data.email,
          userName:
            response.data.userName,
          roles:
            response.data.roles,
        });
      } catch {
        setAccessToken(null);
        setUser(null);
      } finally {
        setIsLoading(false);
      }
    };

    restoreSession();
  }, []);

  const refreshAccessToken = useCallback(
    async () => {
      try {
        const response =
          await apiClient.post(
            "/api/auth/refresh"
          );

        setAccessToken(
          response.data.accessToken
        );

        setUser({
          email: response.data.email,
          userName:
            response.data.userName,
          roles:
            response.data.roles,
        });

        return response.data.accessToken;
      } catch (error) {
        setAccessToken(null);
        setUser(null);
        throw error;
      }
    },
    []
  );

  const login = async (email, password) => {
    const response =
      await apiClient.post(
        "/api/auth/login",
        {
          email,
          password,
        }
      );

    setAccessToken(
      response.data.accessToken
    );

    setUser({
      email: response.data.email,
      userName:
        response.data.userName,
      roles:
        response.data.roles,
    });

    return response.data;
  };

  const logout = useCallback(
    async ({ skipRequest } = {}) => {
      if (!skipRequest) {
        try {
          await apiClient.post(
            "/api/auth/logout"
          );
        } catch {
          // ignore logout errors
        }
      }

      setAccessToken(null);
      setUser(null);
    },
    []
  );

  useEffect(() => {
    const eject =
      setupInterceptors(
        () => accessToken,
        refreshAccessToken,
        logout
      );

    return () => eject();
  }, [accessToken, refreshAccessToken, logout]);

  const value = useMemo(
    () => ({
      accessToken,
      user,
      isLoading,
      login,
      logout,
      setAccessToken,
    }),
    [
      accessToken,
      user,
      isLoading,
    ]
  );

  return (
    <AuthContext.Provider
      value={value}
    >
      {children}
    </AuthContext.Provider>
  );
}

// eslint-disable-next-line react-refresh/only-export-components
export function useAuth() {
  const context =
    useContext(AuthContext);

  if (!context) {
    throw new Error(
      "useAuth must be used within AuthProvider"
    );
  }

  return context;
}