export default function GameCard({
  game,
}) {
  return (
    <article className="game-card">
      <h2>{game.title}</h2>

      <p>{game.genre}</p>

      <p>${game.price}</p>

      <p>
        Released:
        {" "}
        {game.releaseDate}
      </p>
    </article>
  );
}