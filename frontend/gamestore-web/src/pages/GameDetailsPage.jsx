import { Link, useParams }
  from "react-router-dom";

import { useGameById }
  from "../hooks/games/useGameById";

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
  } = useGameById(id);

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
        {data.releaseDateUtc
          ? new Date(
              data.releaseDateUtc
            ).toLocaleDateString()
          : "-"}
      </p>
    </div>
  );
}