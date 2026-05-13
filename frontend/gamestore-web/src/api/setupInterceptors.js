import { apiClient } from "./client";
// Function to set up Axios interceptors for attaching JWT tokens to requests
export function setupInterceptors(getAccessToken) {
  apiClient.interceptors.request.use(
    (config) => {
      const token = getAccessToken();

      if (token) {
        config.headers.Authorization =
          `Bearer ${token}`;
      }

      return config;
    }
  );
}