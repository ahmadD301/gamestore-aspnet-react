import { Link }
from "react-router-dom";

import LoadingSpinner
from "../components/common/LoadingSpinner";

import ErrorBanner
from "../components/common/ErrorBanner";

import { useGames }
from "../hooks/games/useGames";

import { useDeleteGame }
from "../hooks/games/useDeleteGame";

export default function AdminGamesPage() {
  const {
    data,
    isLoading,
    isError,
    error,
  } = useGames({
    page: 1,
    pageSize: 100,
  });

  const deleteMutation =
    useDeleteGame();

  async function handleDelete(id) {
    const confirmed =
      window.confirm(
        "Delete this game?"
      );

    if (!confirmed) {
      return;
    }

    deleteMutation.mutate(id);
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
    <div>
      <div className="admin-header">
        <h1>Manage Games</h1>

        <Link to="/admin/games/new">
          Add Game
        </Link>
      </div>

        <table
          className="admin-table"
          aria-label="Games management table"
        >
        <thead>
          <tr>
            <th scope="col">Title</th>
            <th scope="col">Genre</th>
            <th scope="col">Price</th>
            <th scope="col">Created By</th>
            <th scope="col">Updated By</th>
            <th scope="col">Actions</th>
          </tr>
        </thead>

        <tbody>
          {data.items.map((game) => (
            <tr key={game.id}>
              <td>{game.title}</td>

              <td>{game.genre}</td>

              <td>${game.price}</td>

              <td>
                {game.createdBy
                  ?? "-"}
              </td>

              <td>
                {game.updatedBy
                  ?? "-"}
              </td>

              <td>
                <Link
                  to={`/admin/games/${game.id}/edit`}
                >
                  Edit
                </Link>

                <button
                  onClick={() =>
                    handleDelete(
                      game.id
                    )
                  }
                >
                  Delete
                </button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}