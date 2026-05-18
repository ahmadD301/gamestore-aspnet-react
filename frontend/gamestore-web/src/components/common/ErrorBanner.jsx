export default function ErrorBanner({
  message,
  onDismiss,
}) {
  return (
    <div
      className="error-banner"
      role="alert"
      aria-live="assertive"
    >
      <span className="error-icon" aria-hidden="true" />
      <span>{message}</span>
      {onDismiss && (
        <button
          type="button"
          className="icon-button"
          aria-label="Dismiss error"
          onClick={onDismiss}
        >
          <span aria-hidden="true">x</span>
        </button>
      )}
    </div>
  );
}