export default function LoadingSpinner() {
  return (
    <div
      className="loading-spinner"
      role="status"
      aria-live="polite"
      aria-label="Loading content"
    >
      <span className="sr-only">
        Loading...
      </span>

      <div
        className="spinner"
        aria-hidden="true"
      />
    </div>
  );
}