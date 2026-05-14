import { Link, useParams }
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
  } = useGames();

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
    <div className="game-details">
      <Link to="/games">
        ← Back to catalog
      </Link>

      <h1>{data.title}</h1>

      <span className="genre-badge">
        {data.genre}
      </span>

      <p>{data.description}</p>

      <h2>${data.price}</h2>

      <p>
        Released:
        {" "}
        {data.releaseDate}
      </p>
    </div>
  );
}