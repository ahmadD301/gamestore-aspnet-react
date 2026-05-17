import { apiClient }
  from "./client";

export function setupInterceptors(
  getAccessToken,
  refreshAccessToken,
  logout
) {
  const requestInterceptor =
    apiClient.interceptors.request.use(
      (config) => {
        const token =
          getAccessToken();

        if (token) {
          config.headers.Authorization =
            `Bearer ${token}`;
        }

        return config;
      }
    );

  const responseInterceptor =
    apiClient.interceptors.response.use(
      (response) => response,

      async (error) => {
        const originalRequest =
          error.config;

        const requestUrl =
          originalRequest?.url ?? "";

        const isRefreshRequest =
          requestUrl.includes(
            "/auth/refresh"
          );

        const isLogoutRequest =
          requestUrl.includes(
            "/auth/logout"
          );

        if (
          error.response?.status === 401 &&
          !originalRequest._retry &&
          !isRefreshRequest &&
          !isLogoutRequest
        ) {
          originalRequest._retry = true;

          try {
            const newToken =
              await refreshAccessToken();

            originalRequest.headers.Authorization =
              `Bearer ${newToken}`;

            return apiClient(
              originalRequest
            );
          } catch {
            try {
              await logout({
                skipRequest: true,
              });
            } catch {
              // ignore logout errors
            }
          }
        }

        return Promise.reject(error);
      }
    );

  return () => {
    apiClient.interceptors.request.eject(
      requestInterceptor
    );

    apiClient.interceptors.response.eject(
      responseInterceptor
    );
  };
}