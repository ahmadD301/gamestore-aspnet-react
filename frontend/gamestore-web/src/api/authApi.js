import { apiClient } from "./client";

export async function loginRequest(payload) {
  const response = await apiClient.post(
    "/api/auth/login",
    payload
  );

  return response.data;
}

export async function refreshRequest(refreshToken) {
  const response = await apiClient.post(
    "/api/auth/refresh",
    {
      refreshToken,
    }
  );

  return response.data;
}

export async function logoutRequest(refreshToken) {
  await apiClient.post("/api/auth/logout", {
    refreshToken,
  });
}