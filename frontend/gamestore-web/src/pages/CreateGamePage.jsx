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

  function handleSubmit(payload, action) {
    createMutation.mutate(payload, {
      onSuccess: () => {
        if (action !== "continue") {
          navigate("/admin/games");
        }
      },
    });
  }

  return (
    <div className="page admin-form-page">
      <div className="container">
        <div className="admin-header">
          <div>
            <p className="breadcrumb">Admin / Games</p>
            <h1>Create Game</h1>
          </div>
        </div>

        <div className="card form-card">
          <GameForm
            genres={genres}
            onSubmit={handleSubmit}
            onCancel={() => navigate("/admin/games")}
            isLoading={
              createMutation.isPending
            }
          />
        </div>
      </div>
    </div>
  );
}