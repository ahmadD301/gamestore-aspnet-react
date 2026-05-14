import { apiClient } from "./client";

export async function getGenres() {
  const response =
    await apiClient.get("/api/genres");

  return response.data;
}