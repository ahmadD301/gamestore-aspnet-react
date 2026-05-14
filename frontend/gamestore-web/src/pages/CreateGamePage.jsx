import { useNavigate }
from "react-router-dom";

import GameForm
from "../components/games/GameForm";

import { useGenres }
from "../hooks/games/useGenres";

import { useCreateGame }
from "../hooks/games/useCreateGame";

export default function CreateGamePage() {
  const navigate =
    useNavigate();

  const {
    data: genres = [],
  } = useGenres();

  const createMutation =
    useCreateGame();

  function handleSubmit(payload) {
    createMutation.mutate(payload, {
      onSuccess: () => {
        navigate("/admin/games");
      },
    });
  }

  return (
    <div>
      <h1>Create Game</h1>

      <GameForm
        genres={genres}
        onSubmit={handleSubmit}
        isLoading={
          createMutation.isPending
        }
      />
    </div>
  );
}