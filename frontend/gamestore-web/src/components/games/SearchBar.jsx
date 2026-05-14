export default function SearchBar({
  value,
  onChange,
}) {
  return (
    <div className="search-bar">
      <input
        type="text"
        placeholder="Search games..."
        value={value}
        onChange={(event) =>
          onChange(event.target.value)
        }
        aria-label="Search games"
      />
    </div>
  );
}