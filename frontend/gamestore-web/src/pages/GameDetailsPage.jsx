import { useParams }
  from "react-router-dom";

import { useGames }
  from "../hooks/games/useGames";

import LoadingSpinner
  from "../components/common/LoadingSpinner";

import ErrorBanner
  from "../components/common/ErrorBanner";

export default function GameDetailsPage() {
  const { id } = useParams();

  const {
    data,
    isLoading,
    isError,
    error,
  } = useGames({ id });

  if (isLoading) {
    return <LoadingSpinner />;
  }

  if (isError) {
    return (
      <ErrorBanner
        message={
          error?.response?.data?.detail
          ??
          "Failed to load game."
        }
      />
    );
  }

  return (
    <div>
      <h1>{data.title}</h1>

      <p>{data.description}</p>

      <p>Genre: {data.genre}</p>

      <p>${data.price}</p>
    </div>
  );
}