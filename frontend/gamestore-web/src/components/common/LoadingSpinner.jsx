export default function LoadingSpinner({ overlay }) {
  return (
    <div
      className={`loading-spinner ${overlay ? "overlay" : ""}`}
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