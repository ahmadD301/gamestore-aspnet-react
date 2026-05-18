export default function SearchBar({
  value,
  onChange,
  onClear,
  isLoading,
}) {
  return (
    <div className="search-bar">
      <label
        htmlFor="game-search"
        className="sr-only"
      >
        Search games
      </label>

      <span className="input-icon search-icon" aria-hidden="true" />

      <input
        id="game-search"
        type="text"
        placeholder="Search games..."
        value={value}
        onChange={(event) =>
          onChange(event.target.value)
        }
        autoComplete="off"
        disabled={isLoading}
      />

      {value && onClear && (
        <button
          type="button"
          className="clear-button"
          aria-label="Clear search"
          onClick={onClear}
          disabled={isLoading}
        >
          <span aria-hidden="true">x</span>
        </button>
      )}
    </div>
  );
}