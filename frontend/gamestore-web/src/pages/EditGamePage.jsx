import { useNavigate,
  useParams }
from "react-router-dom";

import LoadingSpinner
from "../components/common/LoadingSpinner";

import ErrorBanner
from "../components/common/ErrorBanner";

import GameForm
from "../components/games/GameForm";

import { useGameById }
from "../hooks/games/useGameById";

import { useGenres }
from "../hooks/games/useGenres";

import { useUpdateGame }
from "../hooks/games/useUpdateGame";

export default function EditGamePage() {
  const { id } = useParams();

  const navigate =
    useNavigate();

  const {
    data: game,
    isLoading,
    isError,
    error,
  } = useGameById(id);

  const {
    data: genres = [],
  } = useGenres();

  const updateMutation =
    useUpdateGame();

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

  function handleSubmit(payload, action) {
    updateMutation.mutate(
      {
        id,
        payload,
      },
      {
        onSuccess: () => {
          if (action !== "continue") {
            navigate(
              "/admin/games"
            );
          }
        },
      }
    );
  }

  return (
    <div className="page admin-form-page">
      <div className="container">
        <div className="admin-header">
          <div>
            <p className="breadcrumb">Admin / Games</p>
            <h1>Edit Game</h1>
          </div>
        </div>

        <div className="card form-card">
          <GameForm
            initialValues={{
              title: game.title,
              description:
                game.description,
              price: game.price,
              coverImageUrl:
                game.coverImageUrl ?? "",
              releaseDate: game.releaseDateUtc
                ? new Date(
                    game.releaseDateUtc
                  )
                    .toISOString()
                    .slice(0, 10)
                : "",
              genreId: game.genreId,
            }}
            genres={genres}
            onSubmit={handleSubmit}
            onCancel={() => navigate("/admin/games")}
            isLoading={
              updateMutation.isPending
            }
          />
        </div>
      </div>
    </div>
  );
}