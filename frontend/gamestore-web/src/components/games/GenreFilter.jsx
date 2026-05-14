export default function GenreFilter({
  genres,
  value,
  onChange,
}) {
  return (
    <div className="genre-filter">
      <select
        value={value}
        onChange={(event) =>
          onChange(event.target.value)
        }
        aria-label="Filter by genre"
      >
        <option value="">
          All Genres
        </option>

        {genres.map((genre) => (
          <option
            key={genre.id}
            value={genre.id}
          >
            {genre.name}
          </option>
        ))}
      </select>
    </div>
  );
}