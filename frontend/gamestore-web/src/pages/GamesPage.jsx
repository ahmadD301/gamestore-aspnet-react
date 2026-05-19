import { useMemo, useState } from "react";
import { Link } from "react-router-dom";

import GameCard
  from "../components/games/GameCard";

import SearchBar
  from "../components/games/SearchBar";

import GenreFilter
  from "../components/games/GenreFilter";

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

  function handleClearFilters() {
    setPage(1);
    setSearch("");
    setGenreId("");
  }

  const totalCount =
    data?.totalItems
    ?? data?.totalCount
    ?? data?.items?.length
    ?? 0;

  const skeletonItems = useMemo(
    () => Array.from({ length: 6 }),
    []
  );

  if (isLoading) {
    return (
      <div className="page games-page">
        <div className="catalog-header sticky">
          <div className="container">
            <div className="catalog-title">
              <h1>Game Catalog</h1>
              <p className="catalog-meta">Loading games...</p>
            </div>

            <div className="catalog-filters">
              <SearchBar
                value={search}
                onChange={handleSearchChange}
                onClear={() => handleSearchChange("")}
                isLoading
              />

              <GenreFilter
                genres={[]}
                value={genreId}
                onChange={handleGenreChange}
                isLoading
              />
            </div>
          </div>
        </div>

        <div className="container">
          <div className="games-grid">
            {skeletonItems.map((_, index) => (
              <div
                key={`skeleton-${index}`}
                className="game-card skeleton"
                aria-hidden="true"
              >
                <div className="game-card-media" />
                <div className="skeleton-line" />
                <div className="skeleton-line short" />
                <div className="skeleton-line" />
              </div>
            ))}
          </div>
        </div>
      </div>
    );
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
    <div className="page games-page">
      <div className="catalog-header sticky">
        <div className="container">
          <div className="catalog-title">
            <h1>Game Catalog</h1>
            <p className="catalog-meta">
              {totalCount} results
            </p>
          </div>

          <div className="catalog-filters">
            <SearchBar
              value={search}
              onChange={handleSearchChange}
              onClear={() => handleSearchChange("")}
            />

            <GenreFilter
              genres={genres}
              value={genreId}
              onChange={handleGenreChange}
            />
          </div>
        </div>
      </div>

      <div className="container">
        {!data?.items?.length ? (
          <EmptyState
            title="No Games Found"
            description={
              "Try adjusting your search or filters."
            }
            actionLabel="Clear Filters"
            onAction={handleClearFilters}
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
                className="button secondary"
                disabled={page <= 1}
                aria-disabled={page <= 1}
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
                className="button secondary"
                disabled={
                  page >= data.totalPages
                }
                aria-disabled={
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
    </div>
  );
}