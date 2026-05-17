import { useNavigate,
  useParams }
from "react-router-dom";

import LoadingSpinner
from "../components/common/LoadingSpinner";

import ErrorBanner
from "../components/common/ErrorBanner";

import GameForm
from "../components/games/GameForm";

import { useGames }
from "../hooks/games/useGames";

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
  } = useGames(id);

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

  function handleSubmit(payload) {
    updateMutation.mutate(
      {
        id,
        payload,
      },
      {
        onSuccess: () => {
          navigate(
            "/admin/games"
          );
        },
      }
    );
  }

  return (
    <div>
      <h1>Edit Game</h1>

      <GameForm
        initialValues={{
          title: game.title,
          description:
            game.description,
          price: game.price,
          releaseDate:  
            game.releaseDate,
        genreId: game.genreId
        }}
        genres={genres}
        onSubmit={handleSubmit}
        isLoading={
          updateMutation.isPending
        }
      />
    </div>
  );
}