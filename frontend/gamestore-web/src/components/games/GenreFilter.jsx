export default function GenreFilter({
  genres,
  value,
  onChange,
  isLoading,
}) {
  return (
    <div className="genre-filter">
      <label
        htmlFor="genre-select"
        className="sr-only"
      >
        Filter by genre
      </label>

      <select
        id="genre-select"
        value={value}
        onChange={(event) =>
          onChange(event.target.value)
        }
        disabled={isLoading}
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

      <span className="select-icon" aria-hidden="true" />
    </div>
  );
}