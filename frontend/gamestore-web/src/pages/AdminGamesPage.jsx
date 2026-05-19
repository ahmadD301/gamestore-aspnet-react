import { useMemo, useState }
from "react";
import { Link }
from "react-router-dom";
import { Pencil, Trash } from "lucide-react";

import LoadingSpinner
from "../components/common/LoadingSpinner";

import ErrorBanner
from "../components/common/ErrorBanner";

import { useGames }
from "../hooks/games/useGames";

import { useDeleteGame }
from "../hooks/games/useDeleteGame";

import SearchBar
from "../components/games/SearchBar";

import GenreFilter
from "../components/games/GenreFilter";

import { useGenres }
from "../hooks/games/useGenres";

export default function AdminGamesPage() {
  const [page, setPage] = useState(1);
  const [search, setSearch] = useState("");
  const [genreId, setGenreId] = useState("");
  const [selectedIds, setSelectedIds] = useState([]);
  const [sortKey, setSortKey] = useState("title");
  const [sortDir, setSortDir] = useState("asc");
  const [pendingDeleteIds, setPendingDeleteIds] = useState([]);

  const {
    data,
    isLoading,
    isError,
    error,
  } = useGames({
    page,
    pageSize: 10,
    search,
    genreId,
  });

  const deleteMutation =
    useDeleteGame();

  const { data: genres = [] } = useGenres();

  const items = data?.items ?? [];

  const sortedItems = useMemo(() => {
    const clone = [...items];
    clone.sort((a, b) => {
      const aValue = a[sortKey] ?? "";
      const bValue = b[sortKey] ?? "";
      if (aValue < bValue) return sortDir === "asc" ? -1 : 1;
      if (aValue > bValue) return sortDir === "asc" ? 1 : -1;
      return 0;
    });
    return clone;
  }, [items, sortDir, sortKey]);

  const allSelected =
    sortedItems.length > 0
    && selectedIds.length === sortedItems.length;

  function handleToggleSelect(id) {
    setSelectedIds((prev) =>
      prev.includes(id)
        ? prev.filter((item) => item !== id)
        : [...prev, id]
    );
  }

  function handleToggleAll() {
    if (allSelected) {
      setSelectedIds([]);
      return;
    }

    setSelectedIds(sortedItems.map((game) => game.id));
  }

  function handleSort(nextKey) {
    if (nextKey === sortKey) {
      setSortDir((prev) => (prev === "asc" ? "desc" : "asc"));
      return;
    }

    setSortKey(nextKey);
    setSortDir("asc");
  }

  function openDeleteModal(ids) {
    setPendingDeleteIds(ids);
  }

  function closeDeleteModal() {
    setPendingDeleteIds([]);
  }

  function confirmDelete() {
    pendingDeleteIds.forEach((id) => {
      deleteMutation.mutate(id);
    });
    setSelectedIds((prev) =>
      prev.filter((id) => !pendingDeleteIds.includes(id))
    );
    closeDeleteModal();
  }

  if (isLoading) {
    return <LoadingSpinner />;
  }

  if (isError) {
    return (
      <ErrorBanner
        message={
          error?.response?.data
            ?.detail
        }
      />
    );
  }

  return (
    <div className="page admin-games-page">
      <div className="container">
        <div className="admin-header">
          <div>
            <p className="breadcrumb">Admin / Games</p>
            <h1>Manage Games</h1>
          </div>

          <div className="admin-actions">
            <Link className="button primary" to="/admin/games/new">
              Add Game
            </Link>
          </div>
        </div>

        <div className="admin-toolbar">
          <SearchBar
            value={search}
            onChange={(value) => {
              setPage(1);
              setSearch(value);
            }}
            onClear={() => {
              setPage(1);
              setSearch("");
            }}
          />

          <GenreFilter
            genres={genres}
            value={genreId}
            onChange={(value) => {
              setPage(1);
              setGenreId(value);
            }}
          />

          <button
            type="button"
            className="button danger"
            disabled={selectedIds.length === 0}
            aria-disabled={selectedIds.length === 0}
            onClick={() => openDeleteModal(selectedIds)}
          >
            Delete Selected
          </button>
        </div>

        <div className="admin-table-wrapper">
          <table
            className="admin-table"
            aria-label="Games management table"
          >
            <thead>
              <tr>
                <th scope="col">
                  <input
                    type="checkbox"
                    aria-label="Select all games"
                    checked={allSelected}
                    onChange={handleToggleAll}
                  />
                </th>
                <th scope="col">
                  <button
                    type="button"
                    className="table-sort"
                    onClick={() => handleSort("title")}
                  >
                    Title
                    <span
                      className={`sort-indicator ${sortKey === "title" ? sortDir : ""}`}
                      aria-hidden="true"
                    />
                  </button>
                </th>
                <th scope="col">
                  <button
                    type="button"
                    className="table-sort"
                    onClick={() => handleSort("genre")}
                  >
                    Genre
                    <span
                      className={`sort-indicator ${sortKey === "genre" ? sortDir : ""}`}
                      aria-hidden="true"
                    />
                  </button>
                </th>
                <th scope="col">
                  <button
                    type="button"
                    className="table-sort"
                    onClick={() => handleSort("price")}
                  >
                    Price
                    <span
                      className={`sort-indicator ${sortKey === "price" ? sortDir : ""}`}
                      aria-hidden="true"
                    />
                  </button>
                </th>
                <th scope="col">Created By</th>
                <th scope="col">Updated By</th>
                <th scope="col">Actions</th>
              </tr>
            </thead>

            <tbody>
              {sortedItems.map((game) => (
                <tr key={game.id}>
                  <td>
                    <input
                      type="checkbox"
                      aria-label={`Select ${game.title}`}
                      checked={selectedIds.includes(game.id)}
                      onChange={() => handleToggleSelect(game.id)}
                    />
                  </td>

                  <td>{game.title}</td>

                  <td>{game.genre}</td>

                  <td>${game.price}</td>

                  <td>
                    {game.createdBy ?? "-"}
                  </td>

                  <td>
                    {game.updatedBy ?? "-"}
                  </td>

                  <td className="admin-actions-cell">
                    <Link
                      className="icon-button"
                      to={`/admin/games/${game.id}/edit`}
                      aria-label={`Edit ${game.title}`}
                    >
                      <Pencil className="icon edit" aria-hidden="true" />
                    </Link>

                    <button
                      type="button"
                      className="icon-button danger"
                      onClick={() => openDeleteModal([game.id])}
                      aria-label={`Delete ${game.title}`}
                    >
                      <Trash className="icon delete" aria-hidden="true" />
                    </button>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>

        <div className="admin-cards">
          {sortedItems.map((game) => (
            <article key={game.id} className="admin-card">
              <div className="admin-card-header">
                <h3>{game.title}</h3>
                <span className="genre-badge">{game.genre}</span>
              </div>
              <p className="muted">${game.price}</p>
              <div className="admin-card-meta">
                <span>Created by {game.createdBy ?? "-"}</span>
                <span>Updated by {game.updatedBy ?? "-"}</span>
              </div>
              <div className="admin-card-actions">
                <Link
                  className="button secondary"
                  to={`/admin/games/${game.id}/edit`}
                >
                  Edit
                </Link>
                <button
                  type="button"
                  className="button danger"
                  onClick={() => openDeleteModal([game.id])}
                >
                  Delete
                </button>
              </div>
            </article>
          ))}
        </div>

        <div className="pagination">
          <button
            className="button secondary"
            disabled={page <= 1}
            aria-disabled={page <= 1}
            onClick={() => setPage((prev) => prev - 1)}
          >
            Previous
          </button>
          <span>
            Page {data?.page ?? 1} of {data?.totalPages ?? 1}
          </span>
          <button
            className="button secondary"
            disabled={page >= (data?.totalPages ?? 1)}
            aria-disabled={page >= (data?.totalPages ?? 1)}
            onClick={() => setPage((prev) => prev + 1)}
          >
            Next
          </button>
        </div>
      </div>

      {pendingDeleteIds.length > 0 && (
        <div className="modal-backdrop" role="presentation">
          <div
            className="modal"
            role="dialog"
            aria-modal="true"
            aria-labelledby="delete-title"
          >
            <h2 id="delete-title">Confirm deletion</h2>
            <p>
              This action cannot be undone. Delete {pendingDeleteIds.length}
              {" "}
              selected game{pendingDeleteIds.length > 1 ? "s" : ""}?
            </p>
            <div className="modal-actions">
              <button
                type="button"
                className="button secondary"
                onClick={closeDeleteModal}
              >
                Cancel
              </button>
              <button
                type="button"
                className="button danger"
                onClick={confirmDelete}
              >
                Delete
              </button>
            </div>
          </div>
        </div>
      )}
    </div>
  );
}