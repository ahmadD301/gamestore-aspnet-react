export default function GameCard({
  game,
}) {
  return (
    <article className="game-card">
      <div className="game-card-media">
        {game.coverImageUrl ? (
          <img
            src={game.coverImageUrl}
            alt={game.title}
            loading="lazy"
          />
        ) : (
          <div className="media-shine" aria-hidden="true" />
        )}
      </div>

      <div className="game-card-body">
        <div className="game-card-header">
          <h2>{game.title}</h2>

          <span className="genre-badge">
            {game.genre}
          </span>
        </div>

        <p className="game-description">
          {game.description}
        </p>
      </div>

      <div className="game-card-footer">
        <strong className="price">
          ${game.price}
        </strong>

        <span className="release-date">
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