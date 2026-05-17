import { useState } from "react";
import { Link } from "react-router-dom";

import GameCard
  from "../components/games/GameCard";

import SearchBar
  from "../components/games/SearchBar";

import GenreFilter
  from "../components/games/GenreFilter";

import LoadingSpinner
  from "../components/common/LoadingSpinner";

import ErrorBanner
  from "../components/common/ErrorBanner";

import EmptyState
  from "../components/common/EmptyState";

import { useGames }
  from "../hooks/games/useGames";

import { useGenres }
  from "../hooks/games/useGenres";

import { useDebounce }
  from "../hooks/useDebounce";

export default function GamesPage() {
  const [page, setPage] = useState(1);

  const [search, setSearch] =
    useState("");

  const [genreId, setGenreId] =
    useState("");

  const debouncedSearch =
    useDebounce(search);

  const {
    data,
    isLoading,
    isError,
    error,
  } = useGames({
    page,
    pageSize: 6,
    search: debouncedSearch,
    genreId,
  });

  const {
    data: genres = [],
  } = useGenres();

  function handleSearchChange(value) {
    setPage(1);

    setSearch(value);
  }

  function handleGenreChange(value) {
    setPage(1);

    setGenreId(value);
  }

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

  return (
    <div className="games-page">
      <div className="catalog-header">
        <h1>Game Catalog</h1>

        <div className="catalog-filters">
          <SearchBar
            value={search}
            onChange={
              handleSearchChange
            }
          />

          <GenreFilter
            genres={genres}
            value={genreId}
            onChange={
              handleGenreChange
            }
          />
        </div>
      </div>

      {!data?.items?.length ? (
        <EmptyState
          title="No Games Found"
          description={
            "Try adjusting filters."
          }
        />
      ) : (
        <>
          <div className="games-grid">
            {data.items.map((game) => (
              <Link
                key={game.id}
                to={`/games/${game.id}`}
                className="game-link"
              >
                <GameCard game={game} />
              </Link>
            ))}
          </div>

          <div className="pagination">
            <button
              disabled={page <= 1}
              onClick={() =>
                setPage(
                  (prev) => prev - 1
                )
              }
            >
              Previous
            </button>

            <span>
              Page {data.page}
              {" "}
              of
              {" "}
              {data.totalPages}
            </span>

            <button
              disabled={
                page >= data.totalPages
              }
              onClick={() =>
                setPage(
                  (prev) => prev + 1
                )
              }
            >
              Next
            </button>
          </div>
        </>
      )}
    </div>
  );
}