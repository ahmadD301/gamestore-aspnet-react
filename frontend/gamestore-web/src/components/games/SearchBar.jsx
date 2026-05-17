export default function SearchBar({
  value,
  onChange,
}) {
  return (
    <div className="search-bar">
      <label
        htmlFor="game-search"
        className="sr-only"
      >
        Search games
      </label>

      <input
        id="game-search"
        type="text"
        placeholder="Search games..."
        value={value}
        onChange={(event) =>
          onChange(event.target.value)
        }
        autoComplete="off"
      />
    </div>
  );
}