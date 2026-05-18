export default function GameCard({
  game,
}) {
  return (
    <article className="game-card">
      <div className="game-card-header">
        <h2>{game.title}</h2>

        <span className="genre-badge">
          {game.genre}
        </span>
      </div>

      <p className="game-description">
        {game.description}
      </p>

      <div className="game-card-footer">
        <strong>
          ${game.price}
        </strong>

        <span>
          {game.releaseDateUtc
            ? new Date(
                game.releaseDateUtc
              ).toLocaleDateString()
            : "-"}
        </span>
      </div>
    </article>
  );
}