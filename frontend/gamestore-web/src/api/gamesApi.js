import { apiClient } from "./client";

export async function getGames({
  page = 1,
  pageSize = 10,
  search = "",
  genreId = "",
}) {
  const response =
    await apiClient.get("/api/games", {
      params: {
        page,
        pageSize,
        search,
        genreId,
      },
    });

  return response.data;
}

export async function getGameById(id) {
  const response =
    await apiClient.get(
      `/api/games/${id}`
    );
  return response.data;
}

export async function createGame(payload) {
  const response =
    await apiClient.post(
      "/api/games",
      payload
    );

  return response.data;
}

export async function updateGame(
  id,
  payload
) {
  await apiClient.put(
    `/api/games/${id}`,
    payload
  );
}

export async function deleteGame(id) {
  await apiClient.delete(
    `/api/games/${id}`
  );
}
    