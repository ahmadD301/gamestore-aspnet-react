export default function GenreFilter({
  genres,
  value,
  onChange,
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