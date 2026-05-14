import { useState } from "react";

import GameCard
  from "../components/games/GameCard";

import LoadingSpinner
  from "../components/common/LoadingSpinner";

import ErrorBanner
  from "../components/common/ErrorBanner";

import EmptyState
  from "../components/common/EmptyState";

import { useGames }
  from "../hooks/games/useGames";

export default function GamesPage() {
  const [page, setPage] = useState(1);

  const {
    data,
    isLoading,
    isError,
    error,
  } = useGames({
    page,
    pageSize: 6,
  });

  if (isLoading) {
    return <LoadingSpinner />;
  }

  if (isError) {
    return (
      <ErrorBanner
        message={
          error?.response?.data?.detail
          ??
          "Failed to load games."
        }
      />
    );
  }

  if (!data?.items?.length) {
    return (
      <EmptyState
        title="No Games Found"
        description="Try adding games."
      />
    );
  }

  return (
    <div>
      <h1>Games</h1>

      <div className="games-grid">
        {data.items.map((game) => (
          <GameCard
            key={game.id}
            game={game}
          />
        ))}
      </div>

      <div className="pagination">
        <button
          disabled={page <= 1}
          onClick={() =>
            setPage((prev) => prev - 1)
          }
        >
          Previous
        </button>

        <span>
          Page {data.page}
        </span>

        <button
          disabled={
            page >= data.totalPages
          }
          onClick={() =>
            setPage((prev) => prev + 1)
          }
        >
          Next
        </button>
      </div>
    </div>
  );
}